using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceFlow.Application.Common.Exceptions;

namespace ServiceFlow.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType =
            "application/problem+json";

        switch (exception)
        {
            case ValidationException validationException:
            {
                var errors = validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .Distinct()
                            .ToArray());

                var problemDetails =
                    new ValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed.",
                        Detail =
                            "One or more validation errors occurred.",
                        Instance = httpContext.Request.Path
                    };

                problemDetails.Extensions["traceId"] =
                    httpContext.TraceIdentifier;

                httpContext.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsJsonAsync(
                    problemDetails,
                    cancellationToken);

                return true;
            }

            case NotFoundException:
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                };

                problemDetails.Extensions["traceId"] =
                    httpContext.TraceIdentifier;

                httpContext.Response.StatusCode =
                    StatusCodes.Status404NotFound;

                await httpContext.Response.WriteAsJsonAsync(
                    problemDetails,
                    cancellationToken);

                return true;
            }

            case ConflictException:
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict.",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                };

                problemDetails.Extensions["traceId"] =
                    httpContext.TraceIdentifier;

                httpContext.Response.StatusCode =
                    StatusCodes.Status409Conflict;

                await httpContext.Response.WriteAsJsonAsync(
                    problemDetails,
                    cancellationToken);

                return true;
            }

            default:
            {
                _logger.LogError(
                    exception,
                    "An unhandled exception occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Detail =
                        "The server encountered an unexpected error.",
                    Instance = httpContext.Request.Path
                };

                problemDetails.Extensions["traceId"] =
                    httpContext.TraceIdentifier;

                httpContext.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsJsonAsync(
                    problemDetails,
                    cancellationToken);

                return true;
            }
        }
    }
}
