namespace SERP.Api.Common.Exceptions.Handlers
{
    public class ExceptionHandlerFactory
    {
        private readonly IEnumerable<IExceptionHandler> _handlers;

        public ExceptionHandlerFactory(IEnumerable<IExceptionHandler> handlers)
        {
            _handlers = handlers;
        }

        public IExceptionHandler GetHandler(Exception ex)
        {
            return _handlers.FirstOrDefault(handler => handler.CanHandle(ex));
        }
    }
}
