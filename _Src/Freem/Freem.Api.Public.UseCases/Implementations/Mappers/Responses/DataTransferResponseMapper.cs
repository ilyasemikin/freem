using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Domain.UseCases.Contracts.Export;
using Freem.Domain.UseCases.Contracts.Formats.List;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiListFormatsResponse = Freem.Api.Public.Contracts.DTO.DataTransfer.ListFormatsResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class DataTransferResponseMapper
{
    public static IActionResult Map(this Result<ExportDataResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        return new EmptyResult();
    }

    public static IActionResult Map(this Result<ListFormatsResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var exportFormats = result.Value.ExportFormats
            .Select(format => format.Map())
            .ToArray();
        var importFormats = result.Value.ImportFormats
            .Select(format => format.Map())
            .ToArray();
        var response = new ApiListFormatsResponse(exportFormats, importFormats);
        return new OkObjectResult(response);
    }
}
