using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class RecordIdentifier : StringIdentifier, IEquatable<RecordIdentifier>
{
    public RecordIdentifier(string value) 
        : base(value)
    {
    }
    
    public bool Equals(RecordIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static bool operator ==(RecordIdentifier left, RecordIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RecordIdentifier left, RecordIdentifier right)
    {
        return !(left == right);
    }
}
