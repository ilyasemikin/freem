using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Identifiers.Base;

public abstract class StringIdentifier : IEntityIdentifier
{
    public string Value { get; }

    protected internal StringIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not StringIdentifier other)
            return false;

        return other.GetType() == GetType() && other.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static explicit operator string(StringIdentifier identifier) => identifier.Value;
}