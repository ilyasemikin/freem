using Freem.Domain.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportRowEntity : IAuditableEntity
{
    public required string Id { get; init; }
    public required string DocumentId { get; set; }
    public required string SourceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset FirstStartAt { get; set; }

    public ImportDocumentEntity? Document { get; set; }
    public ICollection<ImportRowPeriodEntity>? Periods { get; set; }
    public ICollection<ImportRowActivityEntity>? Activities { get; set; }
    public ICollection<ImportRowTagEntity>? Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
