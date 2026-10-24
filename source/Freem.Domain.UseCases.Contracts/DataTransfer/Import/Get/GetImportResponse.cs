using Freem.Domain.Entities.DataTransfer.Import;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.Get;

public sealed class GetImportResponse
{
    public ImportDocument Document { get; }

    public GetImportResponse(ImportDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        Document = document;
    }
}
