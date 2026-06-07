namespace TaskManager.Api.Middleware;

public class RequestIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestIdMiddleware> _logger;

    public RequestIdMiddleware(RequestDelegate next, ILogger<RequestIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string requestId = context.Request.Headers["X-Request-Id"].FirstOrDefault()
                           ?? Guid.NewGuid().ToString();

        context.Items["RequestId"] = requestId;
        context.Response.Headers["X-Request-Id"] = requestId;

        _logger.LogInformation(
            "event=request_started request_id={RequestId} method={Method} path={Path}",
            requestId,
            context.Request.Method,
            context.Request.Path
        );

        await _next(context);

        _logger.LogInformation(
            "event=request_finished request_id={RequestId} status_code={StatusCode}",
            requestId,
            context.Response.StatusCode
        );
    }
}
