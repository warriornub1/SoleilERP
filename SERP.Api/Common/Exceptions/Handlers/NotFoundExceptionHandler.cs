using SERP.Api.Common.APIMessages;
using SERP.Application.Common.Exceptions;
using System.Net;

namespace SERP.Api.Common.Exceptions.Handlers
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        public bool CanHandle(Exception ex) => ex is NotFoundException;

        public async Task HandleAsync(HttpContext context, Exception ex)
        {
            var notFoundEx = ex as NotFoundException;
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var errorResponse = new APIResponse();
            errorResponse.isSuccess = false;
            errorResponse.AddError(notFoundEx.ErrorCode, notFoundEx.Message);

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
