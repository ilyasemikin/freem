using Freem.Domain.Entities.DataTransfer.Import;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.Upload;

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
