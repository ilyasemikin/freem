namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentActivityTagEntity
{
    public required string DocumentId { get; init; }
    public required string ActivitySourceId { get; init; }
    public required string TagSourceId { get; init; }

    public ImportDocumentActivityEntity? Activity { get; set; }
}
