namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentTagEntity
{
    public required string DocumentId { get; init; }
    public required string SourceId { get; init; }
    public required string Name { get; set; }
    public bool CreateNew { get; set; }
    public string? ResolvedTagId { get; set; }

    public ImportDocumentEntity? Document { get; set; }
    public TagEntity? ResolvedTag { get; set; }
}
