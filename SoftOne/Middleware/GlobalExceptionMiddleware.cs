using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftOne.Exceptions;

namespace SoftOne.Middleware;

public class GlobalExceptionMiddleware
{
    private const string DebugLogPath = "";

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment environment)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var original = exception;
        exception = ExceptionHelper.Normalize(exception);

        var statusCode = ExceptionHelper.ResolveStatusCode(exception);
        var detail = ExceptionHelper.ResolveDetail(exception, false);
        var errorCode = ExceptionHelper.ResolveErrorCode(exception);
        var rootMessage = ExceptionHelper.GetRootMessage(original);

        try
        {
            var payload = JsonSerializer.Serialize(new
            {
          
                location = "GlobalExceptionMiddleware.HandleExceptionAsync",
                message = "Exception handled",
                data = new
                {
                    path = context.Request.Path.Value,
                    method = context.Request.Method,
                    statusCode,
                    errorCode,
                    originalType = original.GetType().Name,
                    normalizedType = exception.GetType().Name,
                    rootMessage,
                    clientDetail = detail,
                    isDevelopment = _environment.IsDevelopment()
                },
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            File.AppendAllText(DebugLogPath, payload + Environment.NewLine);
        }
        catch { }

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(original, "Unhandled exception for {Method} {Path}. Root: {RootMessage}",
                context.Request.Method, context.Request.Path, rootMessage);
        }
        else
        {
            _logger.LogWarning(original, "Handled exception for {Method} {Path}: {Message}",
                context.Request.Method, context.Request.Path, exception.Message);
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ExceptionHelper.ResolveTitle(statusCode),
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["errorCode"] = errorCode;

        if (exception is ValidationException validationException)
        {
            problem.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        if (_environment.IsDevelopment())
        {
            problem.Extensions["developerMessage"] = rootMessage;
            problem.Extensions["exceptionType"] = original.GetType().Name;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
    }
}
