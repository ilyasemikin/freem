using Freem.Domain.Import;

namespace Freem.Domain.Entities.Import;

public sealed class ImportRow :
    IEntity<ImportRowId>
{
    public ImportRowId Id { get; }
    public ImportDocumentId DocumentId { get; }
    public ImportRecord Record { get; }

    public ImportRow(
        ImportRowId id,
        ImportDocumentId documentId,
        ImportRecord record)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(record);

        if (record.Periods.Count == 0)
            throw new ArgumentException("Record must have at least one period", nameof(record));

        Id = id;
        DocumentId = documentId;
        Record = record;
    }
}
