using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.RunningRecords.Identifiers;

public sealed class RunningRecordIdentifier : StringIdentifier, IEntityIdentifier
{
    public RunningRecordIdentifier(string value) 
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

    public static implicit operator RunningRecordIdentifier(string value)
    {
        return new RunningRecordIdentifier(value);
    }

    public static implicit operator RunningRecordIdentifier(UserIdentifier identifier)
    {
        return new RunningRecordIdentifier(identifier);
    }

    public static implicit operator UserIdentifier(RunningRecordIdentifier identifier)
    {
        return new UserIdentifier(identifier);
    }
}