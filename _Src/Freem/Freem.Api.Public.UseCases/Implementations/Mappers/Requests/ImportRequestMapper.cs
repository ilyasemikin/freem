using Freem.Api.Public.UseCases.Contracts.Imports;
using Freem.Domain;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Domain.UseCases.Contracts.Imports.Get;
using Freem.Domain.UseCases.Contracts.Imports.List;
using Freem.Domain.UseCases.Contracts.Imports.ListRows;
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

    public static ListImportRequest Map(this ApiListImportRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportRequest(limit, offset);
    }

    public static ListImportRowRequest Map(this ApiListImportRowRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListImportRowRequest(request.Id, limit, offset);
    }
}
