using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class CategoryIdentifier : StringIdentifier, IEquatable<CategoryIdentifier>
{
    public CategoryIdentifier(string value) 
        : base(value)
    {
    }
    
    public bool Equals(CategoryIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is CategoryIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(CategoryIdentifier left, CategoryIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CategoryIdentifier left, CategoryIdentifier right)
    {
        return !(left == right);
    }
}
