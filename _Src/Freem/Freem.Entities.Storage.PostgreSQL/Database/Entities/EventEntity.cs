using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class EventEntity : IAuditableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string EntityName { get; init; }
    public required string EntityId { get; init; }
    
    public required string Action { get; init; }
    
    public string? AdditionalData { get; init; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}