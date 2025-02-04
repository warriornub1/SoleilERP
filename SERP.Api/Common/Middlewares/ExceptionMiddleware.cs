using Newtonsoft.Json;
using SERP.Api.Common.APIMessages;
using SERP.Api.Common.Exceptions.Handlers;
using SERP.Application.Common.Constants;
using System.Net;

namespace SERP.Api.Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerFactory _handlerFactory;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next, ExceptionHandlerFactory handlerFactory)
        {
            _logger = logger;
            _next = next;
            _handlerFactory = handlerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var existingBody = context.Response.Body;
                var newBody = new MemoryStream();
                context.Response.Body = newBody;

                try
                {
                    await _next(context);
                    if (context.Response.StatusCode == 200 && !IsFileResponse(context))
                    {
                        var newResponse = await FormatResponse(context.Response);
                        newBody.Seek(0, SeekOrigin.Begin);

                        context.Response.Clear();
                        await context.Response.WriteAsync(newResponse);
                    }
                }
                finally
                {
                    newBody.Seek(0, SeekOrigin.Begin);
                    await newBody.CopyToAsync(existingBody);
                    context.Response.Body = existingBody;
                }
            }
            catch (Exception ex)
            {
                var handler = _handlerFactory.GetHandler(ex);
                if (handler != null)
                {
                    await handler.HandleAsync(context, ex);
                }
                else
                {
                    _logger.LogError(ex, ex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var errorResponse = new APIResponse();
                    errorResponse.isSuccess = false;
                    errorResponse.AddError(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);

                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var content = await new StreamReader(response.Body).ReadToEndAsync();
            var customResponse = new APIResponse
            {
                isSuccess = response.StatusCode == 200
            };

            try
            {
                customResponse.result = JsonConvert.DeserializeObject(content);
            }
            catch (JsonReaderException)
            {
                customResponse.result = content;
            }

            var json = JsonConvert.SerializeObject(customResponse);

            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{json}";
        }

        private bool IsFileResponse(HttpContext context)
        {
            var response = context.Response;
            if (response.ContentType != null && response.ContentType.StartsWith("application/"))
            {
                var contentDisposition = context.Response.Headers["Content-Disposition"].ToString();
                if (!string.IsNullOrEmpty(contentDisposition) && contentDisposition.Contains("attachment"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
