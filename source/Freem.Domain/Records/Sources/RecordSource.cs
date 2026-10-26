namespace Freem.Domain.Records.Sources;

/// <summary>
/// Describes how a record came to exist. See <see cref="ManualRecordSource"/>, <see cref="RunningRecordSource"/>
/// and <see cref="ImportRecordSource"/>; no other variant exists.
/// </summary>
public abstract class RecordSource : IEquatable<RecordSource>
{
    public abstract RecordSourceType Type { get; }

    private protected RecordSource()
    {
    }

    public abstract bool Equals(RecordSource? other);

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordSource other && Equals(other);
    }

    public abstract override int GetHashCode();
}
