using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;

namespace Freem.Domain.Records.Sources;

/// <summary>
/// The record was created by applying a row of an import. The document and row may be removed later together
/// with the import; the format and the record key from the file stay meaningful without them.
/// </summary>
public sealed class ImportRecordSource : RecordSource
{
    public ImportDocumentId DocumentId { get; }
    public ImportRowId RowId { get; }
    public FormatKey Format { get; }
    public ImportRecordId ImportRecordId { get; }

    public override RecordSourceType Type => RecordSourceType.Import;

    public ImportRecordSource(
        ImportDocumentId documentId,
        ImportRowId rowId,
        FormatKey format,
        ImportRecordId importRecordId)
    {
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(rowId);
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(importRecordId);

        DocumentId = documentId;
        RowId = rowId;
        Format = format;
        ImportRecordId = importRecordId;
    }

    public override bool Equals(RecordSource? other)
    {
        return other is ImportRecordSource source &&
               DocumentId.Equals(source.DocumentId) &&
               RowId.Equals(source.RowId) &&
               Format.Equals(source.Format) &&
               ImportRecordId.Equals(source.ImportRecordId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, DocumentId, RowId, Format, ImportRecordId);
    }
}
