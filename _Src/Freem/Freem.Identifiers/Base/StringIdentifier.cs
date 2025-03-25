using Freem.Identifiers.Abstractions;

namespace Freem.Identifiers.Base;

public abstract class StringIdentifier : IIdentifier
{
    private readonly string _value;

    protected StringIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _value = value;
    }
    
    public bool Equals(IIdentifier? obj)
    {
        if (obj is null) 
            return false;
        if (ReferenceEquals(this, obj)) 
            return true;
        if (obj.GetType() != GetType()) 
            return false;

        var other = (StringIdentifier)obj;
        return _value == other._value;
    }

    public override bool Equals(object? other)
    {
        return Equals(other as IIdentifier);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(StringIdentifier left, StringIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StringIdentifier left, StringIdentifier right)
    {
        return !(left == right);
    }
    
    public static implicit operator string(StringIdentifier id)
    {
        return id._value;
    }
}