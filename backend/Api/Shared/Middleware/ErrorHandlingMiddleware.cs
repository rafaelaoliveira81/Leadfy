using System.Net;
using System.Text.Json;
using Models.Shared;

namespace Api.Shared.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            if ((int)statusCode >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Erro não tratado na requisição {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                _logger.LogWarning(exception, "Falha de negócio na requisição {Method} {Path}", context.Request.Method, context.Request.Path);
            }

            await WriteErrorResponseAsync(context, exception, statusCode);
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        var response = new ErrorResponse
        {
            Message = exception.Message,
            StatusCode = (int)statusCode,
            StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null,
            InnerException = _environment.IsDevelopment() ? exception.InnerException?.Message : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var payload = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(payload);
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            InvalidOperationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}