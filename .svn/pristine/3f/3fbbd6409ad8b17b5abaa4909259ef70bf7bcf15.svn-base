using SERP.Api.Common.Exceptions.Handlers;
using SERP.Api.Common.Middlewares;

namespace SERP.Api.Common.Exceptions
{
    public static class ExceptionHelper
    {
        public static void AddExceptionServices(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionHandler, BadRequestExceptionHandler>();
            services.AddSingleton<IExceptionHandler, NotFoundExceptionHandler>();
            services.AddSingleton<ExceptionHandlerFactory>();
        }

        public static void ConfigureException(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
