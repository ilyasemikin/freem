using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Users.Models;

public sealed class Nickname : 
    IEquatable<Nickname>, IParsable<Nickname>
{
    private readonly string _value;

    private Nickname(string value)
    {
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

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(Nickname left, Nickname right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Nickname left, Nickname right)
    {
        return !(left == right);
    }

    public static Nickname Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for Nickname: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out Nickname? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new Nickname(s);
        return true;
    }
}