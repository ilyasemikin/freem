namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;

internal abstract class BaseEventEntity
{
    public const int EventTypeMaxLength = 32;
    
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required EventAction Action { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public abstract string EventType { get; protected init; }
}