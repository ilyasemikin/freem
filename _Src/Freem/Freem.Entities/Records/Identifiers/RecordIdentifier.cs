using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Records.Identifiers;

public sealed class RecordIdentifier : StringIdentifier, IEntityIdentifier
{
    public RecordIdentifier(string value)
        : base(value)
    {
    }

    public override bool Equals(object? other)
    {
        return base.Equals(other);
    }

    public bool Equals(IEntityIdentifier? other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static implicit operator RecordIdentifier(string value)
    {
        return new RecordIdentifier(value);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}