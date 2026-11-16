using System.Text.Json;
using System.Text.Json.Serialization;
using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
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
        catch (ValidationFailedException ex)
        {
            await WriteResponseAsync(context, ex.StatusCode, new ErrorResponse
            {
                Message = ex.Message,
                ErrorCode = ex.ErrorCode,
                StatusCode = ex.StatusCode,
                ValidationErrors = ex.ValidationErrors
            });
        }
        catch (AppException ex)
        {
            await WriteResponseAsync(context, ex.StatusCode, new ErrorResponse
            {
                Message = ex.Message,
                ErrorCode = ex.ErrorCode,
                StatusCode = ex.StatusCode
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred.",
                ErrorCode = "INTERNAL_SERVER_ERROR",
                StatusCode = StatusCodes.Status500InternalServerError
            });
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, ErrorResponse body)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
