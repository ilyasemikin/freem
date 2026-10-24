namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentActivityTagEntity
{
    public required string DocumentId { get; init; }
    public required string ImportActivityId { get; init; }
    public required string ImportTagId { get; init; }

    public ImportDocumentActivityEntity? Activity { get; set; }
}
