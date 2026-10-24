namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentTagEntity
{
    public required string DocumentId { get; init; }
    public required string ImportTagId { get; init; }
    public required string Name { get; set; }
    public string? MappedTagId { get; set; }

    public ImportDocumentEntity? Document { get; set; }
    public TagEntity? MappedTag { get; set; }
}
