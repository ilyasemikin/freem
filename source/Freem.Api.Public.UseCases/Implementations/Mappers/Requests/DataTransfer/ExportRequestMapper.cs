using Freem.Api.Public.UseCases.Contracts.DataTransfer.Export;
using Freem.Domain.UseCases.Contracts.DataTransfer.Export;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests.DataTransfer;

internal static class ExportRequestMapper
{
    public static ExportDataRequest Map(this ApiExportDataRequest request)
    {
        return new ExportDataRequest(request.Format, request.OpenOutput);
    }
}
