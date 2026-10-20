using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Web.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiCreateRecordResponse = Freem.Web.Api.Public.Contracts.DTO.Records.CreateRecordResponse;
using RecordResponse = Freem.Web.Api.Public.Contracts.DTO.Records.RecordResponse;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class RecordResponseMapper
{
    public static IActionResult Map(this Result<CreateRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiCreateRecordResponse(result.Value.Record.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<GetRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Record.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var records = result.Value.Records
            .Select(record => record.Map())
            .ToArray();
        return new ItemsListResult<RecordResponse>(records, result.Value.TotalCount);
    }

    public static IActionResult Map(this Result<PeriodListResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var records = result.Value.Records
            .Select(record => record.Map())
            .ToArray();
        return new ItemsListResult<RecordResponse>(records, result.Value.TotalCount);
    }
}
