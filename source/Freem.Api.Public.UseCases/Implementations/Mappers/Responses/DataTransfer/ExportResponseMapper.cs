using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Domain.UseCases.Contracts.DataTransfer.Export;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses.DataTransfer;

internal static class ExportResponseMapper
{
    public static IActionResult Map(this Result<ExportDataResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        return new EmptyResult();
    }
}
