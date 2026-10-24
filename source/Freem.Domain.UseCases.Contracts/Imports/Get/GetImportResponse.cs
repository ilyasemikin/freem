using Freem.Domain.Entities.Import;

namespace Freem.Domain.UseCases.Contracts.Imports.Get;

public sealed class GetImportResponse
{
    public ImportDocument Document { get; }

    public GetImportResponse(ImportDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        Document = document;
    }
}
