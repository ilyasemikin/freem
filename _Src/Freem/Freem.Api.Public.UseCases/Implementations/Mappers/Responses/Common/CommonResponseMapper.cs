using Freem.Domain;
using Freem.Api.Public.Contracts.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;

internal static class CommonResponseMapper
{
    public static int MapStatusCode(ErrorType type)
    {
        if (type == DomainErrorCatalog.Types.NothingToDo ||
            type == DomainErrorCatalog.Types.InvalidRequest)
            return StatusCodes.Status400BadRequest;
        if (type == DomainErrorCatalog.Types.UserNotRegistered ||
            type == DomainErrorCatalog.Types.AuthenticationRejected)
            return StatusCodes.Status401Unauthorized;
        if (type == DomainErrorCatalog.Types.EntityNotFound)
            return StatusCodes.Status404NotFound;
        if (type == DomainErrorCatalog.Types.EntityAlreadyExists ||
            type == LockingErrorCatalog.Types.LockAcquisitionFailed)
            return StatusCodes.Status409Conflict;
        if (type == DomainErrorCatalog.Types.RelatedEntitiesNotFound ||
            type == DomainErrorCatalog.Types.ActivityInvalidStatus ||
            type == DomainErrorCatalog.Types.TagNameAlreadyExists ||
            type == DomainErrorCatalog.Types.PeriodIsInvalid)
            return StatusCodes.Status422UnprocessableEntity;
        if (type == DomainErrorCatalog.Types.FailedDependency)
            return StatusCodes.Status502BadGateway;

        return StatusCodes.Status500InternalServerError;
    }

    public static IActionResult MapError(this Error error)
    {
        var statusCode = MapStatusCode(error.Type);
        return error.MapError(statusCode);
    }

    public static IActionResult MapError(this Error error, int statusCode)
    {
        var response = new ErrorResponse(error);
        return new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }

    public static IActionResult Map(this Result<Error> result)
    {
        return result.Success
            ? new OkResult()
            : result.Error.MapError();
    }
}
