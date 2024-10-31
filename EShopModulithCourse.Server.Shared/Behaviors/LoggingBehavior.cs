using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EShopModulithCourse.Server.Shared.Behaviors;

internal class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestId = $"({Guid.NewGuid().ToString()})";
        _logger.LogInformation("[START] Handling request: {@Request}; Request Data: {@RequestData} -- {@RequestId}",
            request.GetType().Name, request, requestId);

        var startTime = Stopwatch.GetTimestamp();
        var response = await next();
        var deltaSpan = Stopwatch.GetElapsedTime(startTime);

        if (deltaSpan.Seconds > 3)
            _logger.LogWarning("[PERF] Request took {@TimeTaken} seconds -- {@RequestId}", deltaSpan.Seconds,
                requestId);

        _logger.LogInformation("[END] Handled request: {@Request}; Execution Time: {@ElapsedTime}ms -- {@RequestId}",
            request.GetType().Name, deltaSpan.TotalMilliseconds, requestId);
        return response;
    }
}