namespace Freem.Domain.UnitTests.Mocs.Entities;

public sealed class TestId :
    Id,
    IEquatable<TestId>
{
    public TestId(string value)
        : base(value)
    {
    }

    public bool Equals(TestId? other)
    {
        return base.Equals(other);
    }
}