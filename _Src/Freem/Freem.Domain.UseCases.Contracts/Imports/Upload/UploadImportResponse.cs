using Freem.Domain.Entities.Import;

namespace Freem.Domain.UseCases.Contracts.Imports.Upload;

public sealed class UploadImportResponse
{
    public ImportDocument Document { get; }
    public IReadOnlyList<ImportParseWarning> Warnings { get; }

    public UploadImportResponse(ImportDocument document, IReadOnlyList<ImportParseWarning> warnings)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(warnings);

        Document = document;
        Warnings = warnings;
    }
}
