using Freem.Domain.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class RecordPeriodEntity :
    IAuditableEntity,
    IEquatable<RecordPeriodEntity>
{
    public required string Id { get; init; }
    public required string RecordId { get; init; }

    public string? Description { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public RecordEntity? Record { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool Equals(RecordPeriodEntity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            Id == other.Id &&
            RecordId == other.RecordId &&
            Description == other.Description &&
            StartAt.Equals(other.StartAt) &&
            EndAt.Equals(other.EndAt);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordPeriodEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
