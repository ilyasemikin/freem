using Freem.Domain.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Domain.UnitTests.Mocs.Entities;

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