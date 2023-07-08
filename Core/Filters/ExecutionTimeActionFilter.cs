using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Filters;

public class ExecutionTimeActionFilter : IActionFilter {
    private readonly ILogger<ExecutionTimeActionFilter> _logger;
    private readonly Stopwatch _stopwatch;

    public ExecutionTimeActionFilter(ILogger<ExecutionTimeActionFilter> logger) {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    public void OnActionExecuting(ActionExecutingContext context) {
        _logger.LogInformation("OnActionExecuting");
        _stopwatch.Start();
    }

    public void OnActionExecuted(ActionExecutedContext context) {
        _stopwatch.Stop();
        _logger.LogInformation("OnActionExecuted");
        _logger.LogInformation("Execution time: {elapsedTime} ms", _stopwatch.ElapsedMilliseconds);
    }
}