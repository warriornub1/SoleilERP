namespace SERP.Api.Common.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        bool CanHandle(Exception ex);
        Task HandleAsync(HttpContext context, Exception ex);
    }
}
