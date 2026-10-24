using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Api.Public.UseCases.Implementations.Results;
using Freem.Domain.UseCases.Contracts.Imports.Apply;
using Freem.Domain.UseCases.Contracts.Imports.Get;
using Freem.Domain.UseCases.Contracts.Imports.List;
using Freem.Domain.UseCases.Contracts.Imports.ListActivities;
using Freem.Domain.UseCases.Contracts.Imports.ListRows;
using Freem.Domain.UseCases.Contracts.Imports.ListTags;
using Freem.Domain.UseCases.Contracts.Imports.Upload;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiApplyImportResponse = Freem.Api.Public.Contracts.DTO.Imports.ApplyImportResponse;
using ApiUploadImportResponse = Freem.Api.Public.Contracts.DTO.Imports.UploadImportResponse;
using ImportActivityResponse = Freem.Api.Public.Contracts.DTO.Imports.ImportActivityResponse;
using ImportResponse = Freem.Api.Public.Contracts.DTO.Imports.ImportResponse;
using ImportRowResponse = Freem.Api.Public.Contracts.DTO.Imports.ImportRowResponse;
using ImportTagResponse = Freem.Api.Public.Contracts.DTO.Imports.ImportTagResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

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
        var response = new ApiUploadImportResponse(import, warnings);
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
        var response = new ApiApplyImportResponse(value.RecordsCreated, value.SkippedRowIds);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Document.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListImportResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var imports = result.Value.Documents
            .Select(document => document.Map())
            .ToArray();
        return new ItemsListResult<ImportResponse>(imports, result.Value.TotalCount);
    }

    public static IActionResult Map(this Result<ListImportActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        return new ItemsListResult<ImportActivityResponse>(activities, result.Value.TotalCount);
    }

    public static IActionResult Map(this Result<ListImportTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tags = result.Value.Tags
            .Select(tag => tag.Map())
            .ToArray();
        return new ItemsListResult<ImportTagResponse>(tags, result.Value.TotalCount);
    }

    public static IActionResult Map(this Result<ListImportRowResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var rows = result.Value.Rows
            .Select(row => row.Map())
            .ToArray();
        return new ItemsListResult<ImportRowResponse>(rows, result.Value.TotalCount);
    }
}
