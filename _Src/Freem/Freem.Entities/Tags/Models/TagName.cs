namespace Freem.Entities.Models.Tags;

public sealed class TagName : IEquatable<TagName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    public TagName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"Length cannot be more than {MaxLength}", nameof(value));
        
        _value = value;
    }

    public bool Equals(TagName? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TagName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(TagName left, TagName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TagName left, TagName right)
    {
        return !(left == right);
    }

    public static implicit operator TagName(string value)
    {
        return new TagName(value);
    }

    public static implicit operator string(TagName value)
    {
        return value._value;
    }
}