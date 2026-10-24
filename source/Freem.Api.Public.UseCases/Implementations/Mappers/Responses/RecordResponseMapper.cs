using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Records.Dtos;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.Get;
using Freem.Domain.UseCases.Contracts.Records.List;
using Freem.Domain.UseCases.Contracts.Records.ListByPeriod;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiRecordCreateResponse = Freem.Api.Public.Contracts.V1.Records.RecordCreateResponse;
using ApiRecordPeriodAddResponse = Freem.Api.Public.Contracts.V1.Records.RecordPeriodAddResponse;
using RecordDto = Freem.Api.Public.Contracts.V1.Records.Dtos.RecordDto;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class RecordResponseMapper
{
    public static IActionResult Map(this Result<CreateRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiRecordCreateResponse(result.Value.Record.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<AddRecordPeriodResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiRecordPeriodAddResponse(result.Value.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<GetRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var record = result.Value.Record.Map();
        var response = new RecordResponse(record);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListRecordResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var records = result.Value.Records
            .Select(record => record.Map())
            .ToArray();
        var response = new RecordListResponse(result.Value.TotalCount, records);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListRecordByPeriodResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var records = result.Value.Records
            .Select(record => record.Map())
            .ToArray();
        var response = new RecordListResponse(result.Value.TotalCount, records);
        return new OkObjectResult(response);
    }
}
