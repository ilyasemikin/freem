namespace Freem.Domain;

public abstract class Id
{
    private readonly string _value;

    protected Id(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _value = value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Id other && other.GetType() == GetType() && _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    protected static string GenerateValue()
    {
        return Guid.NewGuid().ToString("N");
    }

    public static bool operator ==(Id? left, Id? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Id? left, Id? right)
    {
        return !(left == right);
    }
}
