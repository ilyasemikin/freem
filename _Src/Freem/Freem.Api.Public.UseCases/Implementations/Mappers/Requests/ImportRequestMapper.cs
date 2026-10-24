using Freem.Api.Public.UseCases.Contracts.Imports;
using Freem.Domain;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Domain.UseCases.Contracts.Imports.Apply;
using Freem.Domain.UseCases.Contracts.Imports.Get;
using Freem.Domain.UseCases.Contracts.Imports.List;
using Freem.Domain.UseCases.Contracts.Imports.ListActivities;
using Freem.Domain.UseCases.Contracts.Imports.ListRows;
using Freem.Domain.UseCases.Contracts.Imports.ListTags;
using Freem.Domain.UseCases.Contracts.Imports.UpdateActivityMapping;
using Freem.Domain.UseCases.Contracts.Imports.UpdateTagMapping;
using Freem.Domain.UseCases.Contracts.Imports.Remove;
using Freem.Domain.UseCases.Contracts.Imports.Upload;
using Freem.Domain.Users.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class ImportRequestMapper
{
    public static Result<UploadImportRequest, Error> Map(this ApiUploadImportRequest request)
    {
        if (!FormatKey.TryParse(request.Format, null, out var format))
            return DomainErrorCatalog.Factories.CreateInvalidRequest("Format key is invalid");

        if (!ImportFileName.TryParse(request.FileName, null, out var fileName))
            return DomainErrorCatalog.Factories.CreateInvalidRequest("File name is invalid");

        TimeZoneOffset? timeZoneOffset = null;
        if (!string.IsNullOrWhiteSpace(request.TimeZoneOffset))
        {
            if (!TimeZoneOffset.TryParse(request.TimeZoneOffset, null, out var parsed))
                return DomainErrorCatalog.Factories.CreateInvalidRequest("Time zone offset is invalid");

            timeZoneOffset = parsed;
        }

        return new UploadImportRequest(format, fileName, request.Content, timeZoneOffset);
    }

    public static GetImportRequest Map(this ApiGetImportRequest request)
    {
        return new GetImportRequest(request.Id);
    }

    public static RemoveImportRequest Map(this ApiRemoveImportRequest request)
    {
        return new RemoveImportRequest(request.Id);
    }

    public static UpdateImportActivityMappingRequest Map(this ApiUpdateImportActivityMappingRequest request)
    {
        return new UpdateImportActivityMappingRequest(request.Id, request.ImportActivityId, request.Body.ActivityId);
    }

    public static UpdateImportTagMappingRequest Map(this ApiUpdateImportTagMappingRequest request)
    {
        return new UpdateImportTagMappingRequest(request.Id, request.ImportTagId, request.Body.TagId);
    }

    public static Result<ApplyImportRequest, Error> Map(this ApiApplyImportRequest request)
    {
        var rowIds = request.Body.RowIds;
        if (request.Body.All == rowIds is not null)
            return DomainErrorCatalog.Factories.CreateInvalidRequest("Exactly one of all and rowIds must be set");

        return rowIds is null
            ? ApplyImportRequest.ForAllRows(request.Id)
            : ApplyImportRequest.ForRows(request.Id, rowIds);
    }

    public static ListImportRequest Map(this ApiListImportRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportRequest(limit, offset);
    }

    public static ListImportActivityRequest Map(this ApiListImportActivityRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportActivityRequest(request.Id, limit, offset);
    }

    public static ListImportTagRequest Map(this ApiListImportTagRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportTagRequest(request.Id, limit, offset);
    }

    public static ListImportRowRequest Map(this ApiListImportRowRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportRowRequest(request.Id, limit, offset);
    }
}
