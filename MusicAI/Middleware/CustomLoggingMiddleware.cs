using MusicAI.Contracts.Enums;
using MusicAI.Services.Logs;

namespace MusicAI.Middleware
{
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<CustomLoggingMiddleware> _logger;

        public CustomLoggingMiddleware(RequestDelegate next
            //, ILogger<CustomLoggingMiddleware> logger
            )
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = MyNLog.GetLogger(LoggerType.InfoLogger);
            logger.Debug("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

            await _next(context);

            logger.Debug("Finished handling request.");
        }
    }
}
