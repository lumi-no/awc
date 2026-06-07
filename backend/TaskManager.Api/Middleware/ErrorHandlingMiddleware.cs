using System.Text.Json;
using FluentValidation;
using TaskManager.Api.DTOs;

namespace TaskManager.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            ErrorResponse error = new()
            {
                Code = "VALIDATION_ERROR",
                Message = "Validation failed.",
                Details = ex.Errors.Select(x => x.ErrorMessage).ToList()
            };

            await WriteJson(context, error);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            ErrorResponse error = new()
            {
                Code = "NOT_FOUND",
                Message = ex.Message,
                Details = new List<string>()
            };

            await WriteJson(context, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "event=unhandled_error");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ErrorResponse error = new()
            {
                Code = "INTERNAL_ERROR",
                Message = "Internal server error.",
                Details = new List<string>()
            };

            await WriteJson(context, error);
        }
    }

    private static async Task WriteJson(HttpContext context, ErrorResponse error)
    {
        context.Response.ContentType = "application/json";

        string json = JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
