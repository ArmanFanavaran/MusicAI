using Microsoft.AspNetCore.SignalR;
using MusicAI.Contracts.Enums;
using MusicAI.Services.Logs;

namespace MusicAI.Middleware
{
    public class LoggingHubFilter:IHubFilter
    {
        NLog.ILogger _logger;
        public LoggingHubFilter()
        {
            _logger = MyNLog.GetLogger(LoggerType.InfoLogger);
        }

        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            _logger.Debug("Invoking hub method '{MethodName}' with arguments {Args}", invocationContext.HubMethod.Name, invocationContext.HubMethodArguments);

            try
            {
                var result = await next(invocationContext);

                _logger.Debug("Successfully invoked hub method '{MethodName}'", invocationContext.HubMethod.Name);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug(ex, "Error invoking hub method '{MethodName}'", invocationContext.HubMethod.Name);
                throw;
            }
        }

        public async ValueTask OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, ValueTask> next)
        {
            _logger.Debug("Client connected: {ConnectionId}", context.Context.ConnectionId);
            await next(context);
            _logger.Debug("Client connected: {ConnectionId} completed", context.Context.ConnectionId);
        }

        public async ValueTask OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, ValueTask> next)
        {
            if (exception != null)
            {
                _logger.Debug(exception, "Client disconnected with error: {ConnectionId}", context.Context.ConnectionId);
            }
            else
            {
                _logger.Debug("Client disconnected: {ConnectionId}", context.Context.ConnectionId);
            }

            await next(context, exception);
            _logger.Debug("Client disconnected: {ConnectionId} completed", context.Context.ConnectionId);
        }
    }
}
