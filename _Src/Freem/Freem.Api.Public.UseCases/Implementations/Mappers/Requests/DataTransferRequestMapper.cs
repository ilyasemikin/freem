using Freem.Api.Public.UseCases.Contracts.DataTransfer;
using Freem.Domain.UseCases.Contracts.Export;
using Freem.Domain.UseCases.Contracts.Formats.List;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class DataTransferRequestMapper
{
    public static ExportDataRequest Map(this ApiExportDataRequest request)
    {
        return new ExportDataRequest(request.Format, request.OpenOutput);
    }

    public static ListFormatsRequest Map(this ApiListFormatsRequest request)
    {
        return new ListFormatsRequest();
    }
}
