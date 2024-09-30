using MusicAI.Contracts.Enums;
using MusicAI.Services.Logs;
using System.Net;

namespace MusicAI.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next
            //,ILogger<ExceptionHandlingMiddleware> logger
            )
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var logger = MyNLog.GetLogger(LoggerType.InfoLogger);
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                logger.Debug(ex, "An unhandled exception has occurred while executing the request.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
