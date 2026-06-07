using System.Text.Json;
using TaskManager.Api.DTOs;

namespace TaskManager.Api.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health") ||
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        string? expectedApiKey = _configuration["API_KEY"];

        if (string.IsNullOrWhiteSpace(expectedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteError(context, "CONFIG_ERROR", "API key is not configured.");
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteError(context, "UNAUTHORIZED", "X-API-Key header is required.");
            return;
        }

        if (providedApiKey != expectedApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteError(context, "UNAUTHORIZED", "Invalid API key.");
            return;
        }

        await _next(context);
    }

    private static async Task WriteError(HttpContext context, string code, string message)
    {
        context.Response.ContentType = "application/json";

        ErrorResponse error = new()
        {
            Code = code,
            Message = message,
            Details = new List<string>()
        };

        string json = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(json);
    }
}
