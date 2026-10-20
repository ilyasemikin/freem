using Freem.Domain.UseCases.Contracts.RunningRecords.Get;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class RunningRecordResponseMapper
{
    public static IActionResult Map(this Result<GetRunningRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Record.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<StartRunningRecordResponse, Error> result)
    {
        return result.Success
            ? new OkResult()
            : result.Error.MapError();
    }
}
