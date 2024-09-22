namespace Freem.Entities.Users.Models;

public sealed class Nickname : IEquatable<Nickname>
{
    private readonly string _value;

    public Nickname(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _value = value;
    }

    public bool Equals(Nickname? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Nickname other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(Nickname left, Nickname right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Nickname left, Nickname right)
    {
        return !(left == right);
    }

    public static implicit operator Nickname(string value)
    {
        return new Nickname(value);
    }
    
    public static implicit operator string(Nickname value)
    {
        return value._value;
    }
}