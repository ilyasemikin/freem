using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class TagIdentifier : StringIdentifier, IEquatable<TagIdentifier>
{
    public TagIdentifier(string value) 
        : base(value)
    {
    }

    public bool Equals(TagIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TagIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static bool operator ==(TagIdentifier left, TagIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TagIdentifier left, TagIdentifier right)
    {
        return !(left == right);
    }
}
