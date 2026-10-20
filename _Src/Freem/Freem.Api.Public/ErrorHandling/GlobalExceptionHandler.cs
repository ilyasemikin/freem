using Freem.Domain;
using Freem.Api.Public.Contracts.DTO;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilities.Errors;

namespace Freem.Api.Public.ErrorHandling;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IOptions<JsonOptions> _jsonOptions;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IOptions<JsonOptions> jsonOptions)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(jsonOptions);
        
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception while processing {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        int statusCode;
        Error error;
        if (exception is FormatException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            error = DomainErrorCatalog.Factories.CreateInvalidRequest(exception.Message);
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            error = DomainErrorCatalog.Factories.CreateInternalServerError();
        }

        var response = new ErrorResponse(error);
        
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response, _jsonOptions.Value.JsonSerializerOptions, cancellationToken);

        return true;
    }
}
