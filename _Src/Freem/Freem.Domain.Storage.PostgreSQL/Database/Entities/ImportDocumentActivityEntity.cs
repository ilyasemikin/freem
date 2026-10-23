namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentActivityEntity
{
    public required string DocumentId { get; init; }
    public required string SourceId { get; init; }
    public required string Name { get; set; }
    public string? Color { get; set; }
    public required string Status { get; set; }
    public bool CreateNew { get; set; }
    public string? ResolvedActivityId { get; set; }

    public ImportDocumentEntity? Document { get; set; }
    public ActivityEntity? ResolvedActivity { get; set; }
    public ICollection<ImportDocumentActivityTagEntity>? Tags { get; set; }
}
