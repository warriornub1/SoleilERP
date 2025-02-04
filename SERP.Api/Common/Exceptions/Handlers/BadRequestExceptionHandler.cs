using SERP.Api.Common.APIMessages;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using System.Net;

namespace SERP.Api.Common.Exceptions.Handlers
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        public bool CanHandle(Exception ex) => ex is BadRequestException;

        public async Task HandleAsync(HttpContext context, Exception ex)
        {
            var badRequestEx = ex as BadRequestException;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new APIResponse();
            errorResponse.isSuccess = false;

            if (badRequestEx.ValidationErrors != null)
            {
                foreach (var error in badRequestEx.ValidationErrors)
                {
                    errorResponse.AddError(ErrorCodes.ValidationError, error.Value[0]);
                }
            }
            else
            {
                errorResponse.AddError(badRequestEx.ErrorCode, badRequestEx.Message);

            }
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
