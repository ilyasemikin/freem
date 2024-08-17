using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class UserIdentifier : StringIdentifier
{
    public UserIdentifier(string value) 
        : base(value)
    {
    }
}
