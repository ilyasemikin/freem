using Freem.Entities.Abstractions;
using Freem.Entities.UnitTests.Fixtures.Identifiers;

namespace Freem.Entities.UnitTests.Mocs.Entities;

public sealed class TestEntity : 
    IEntity<TestIdentifier>,
    IEquatable<TestEntity>
{
    public TestIdentifier Id { get; }
    public string Data { get; }

    public TestEntity(TestIdentifier id, string data)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(data);
        
        Id = id;
        Data = data;
    }

    public bool Equals(TestEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TestEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}