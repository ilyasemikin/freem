using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.UnitTests.Mocs.Entities;

public sealed class TestIdentifier : 
    IEntityIdentifier,
    IEquatable<TestIdentifier>
{
    public string Value { get; }
    
    public TestIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        Value = value;
    }

    public bool Equals(TestIdentifier? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public bool Equals(IEntityIdentifier? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TestIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}