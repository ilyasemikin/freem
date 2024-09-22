using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.UnitTests.Mocs.Entities;

public sealed class TestIdentifier :
    StringIdentifier,
    IEntityIdentifier,
    IEquatable<TestIdentifier>
{
    public TestIdentifier(string value)
        : base(value)
    {
    }

    public bool Equals(IEntityIdentifier? other)
    {
        return base.Equals(other);
    }

    public bool Equals(TestIdentifier? other)
    {
        return base.Equals(other);
    }
}