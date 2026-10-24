namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class EventEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string EventType { get; init; }
    public required string EntityType { get; init; }
    public required string EntityRef { get; init; }
    public required DateTimeOffset OccurredAt { get; init; }
    public required string Payload { get; init; }
}
