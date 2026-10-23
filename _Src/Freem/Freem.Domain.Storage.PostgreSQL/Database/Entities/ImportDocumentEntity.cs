using Freem.Domain.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class ImportDocumentEntity : IAuditableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; set; }
    public required string Format { get; set; }
    public required string FileName { get; set; }
    public long TimeZoneOffsetTicks { get; set; }
    public required string Status { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<ImportDocumentTagEntity>? Tags { get; set; }
    public ICollection<ImportDocumentActivityEntity>? Activities { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
