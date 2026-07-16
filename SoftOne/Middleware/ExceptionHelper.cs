using AutoMapper;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SoftOne.Exceptions;

namespace SoftOne.Middleware;


public static class ExceptionHelper
{
    public static Exception Normalize(Exception exception)
    {
        exception = UnwrapWrappers(exception);

        if (exception is DbUpdateConcurrencyException)
        {
            return new ConflictException(ConcurrencyMessages.AlreadyUpdated);
        }

        return exception;
    }

    public static int ResolveStatusCode(Exception exception) => exception switch
    {
        NotFoundException => StatusCodes.Status404NotFound,
        ConflictException => StatusCodes.Status409Conflict,
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        BusinessException => StatusCodes.Status400BadRequest,
        NotSaveException => StatusCodes.Status400BadRequest,
        ValidationException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    public static string ResolveTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status404NotFound => "Not Found",
        StatusCodes.Status409Conflict => "Conflict",
        _ => "An unexpected error occurred"
    };


    public static string ResolveDetail(Exception exception, bool isDevelopment)
    {
        if (exception is ValidationException)
        {
            return "One or more validation errors occurred.";
        }

        if (exception is NotFoundException or ConflictException or UnauthorizedException
            or BusinessException or NotSaveException)
        {
            return exception.Message;
        }

        if (exception is DbUpdateException)
        {
            return isDevelopment
                ? GetRootMessage(exception)
                : "Server Error Please try again later";
        }

        if (GetRootException(exception) is SqlException)
        {
            return isDevelopment
                ? GetRootMessage(exception)
                : "Server Error Please try again later";
        }

        if (statusCodeIsServerError(exception))
        {
            return isDevelopment
                ? $"{GetRootException(exception).GetType().Name}: {GetRootMessage(exception)}"
                : "Server Error Please try again later";
        }

        return exception.Message;
    }

    public static string? ResolveErrorCode(Exception exception) => exception switch
    {
        NotFoundException => "NOT_FOUND",
        ConflictException => "CONFLICT",
        UnauthorizedException => "UNAUTHORIZED",
        BusinessException => "BUSINESS_RULE",
        NotSaveException => "SAVE_FAILED",
        ValidationException => "VALIDATION_FAILED",
        DbUpdateException => "DATABASE_SAVE_FAILED",
        _ when GetRootException(exception) is SqlException => "DATABASE_ERROR",
        _ => "UNEXPECTED_ERROR"
    };

    public static Exception GetRootException(Exception exception)
    {
        exception = UnwrapWrappers(exception);

        var current = exception;
        while (current.InnerException is not null)
        {
            current = current.InnerException;
        }

        return current;
    }

    public static string GetRootMessage(Exception exception) =>
        GetRootException(exception).Message;

    private static bool statusCodeIsServerError(Exception exception) =>
        ResolveStatusCode(exception) >= StatusCodes.Status500InternalServerError;

    private static Exception UnwrapWrappers(Exception exception)
    {
        while (true)
        {
            switch (exception)
            {
                case AutoMapperMappingException mapping when mapping.InnerException is not null:
                    exception = mapping.InnerException;
                    continue;
                case AggregateException aggregate when aggregate.InnerExceptions.Count == 1:
                    exception = aggregate.InnerExceptions[0];
                    continue;
                default:
                    return exception;
            }
        }
    }
}
