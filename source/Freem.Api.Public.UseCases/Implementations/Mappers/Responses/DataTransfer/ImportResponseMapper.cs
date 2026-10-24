using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos.DataTransfer;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Apply;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Get;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.List;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListActivities;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListRows;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListTags;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Upload;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiImportApplyResponse = Freem.Api.Public.Contracts.V1.DataTransfer.Import.ImportApplyResponse;
using ApiImportUploadResponse = Freem.Api.Public.Contracts.V1.DataTransfer.Import.ImportUploadResponse;
using ImportActivityDto = Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos.ImportActivityDto;
using ImportDto = Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos.ImportDto;
using ImportRowDto = Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos.ImportRowDto;
using ImportTagDto = Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos.ImportTagDto;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses.DataTransfer;

internal static class ImportResponseMapper
{
    public static IActionResult Map(this Result<UploadImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var import = result.Value.Document.Map();
        var warnings = result.Value.Warnings
            .Select(warning => warning.Map())
            .ToArray();
        var response = new ApiImportUploadResponse(import, warnings);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<ApplyImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var value = result.Value;
        var response = new ApiImportApplyResponse(value.RecordsCreated, value.SkippedRowIds);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var import = result.Value.Document.Map();
        var response = new ImportResponse(import);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var imports = result.Value.Documents
            .Select(document => document.Map())
            .ToArray();
        var response = new ImportListResponse(result.Value.TotalCount, imports);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListImportActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        var response = new ImportActivityListResponse(result.Value.TotalCount, activities);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListImportTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tags = result.Value.Tags
            .Select(tag => tag.Map())
            .ToArray();
        var response = new ImportTagListResponse(result.Value.TotalCount, tags);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListImportRowResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var rows = result.Value.Rows
            .Select(row => row.Map())
            .ToArray();
        var response = new ImportRowListResponse(result.Value.TotalCount, rows);
        return new OkObjectResult(response);
    }
}
