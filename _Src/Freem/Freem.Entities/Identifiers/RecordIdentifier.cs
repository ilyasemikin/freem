using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class RecordIdentifier : StringIdentifier
{
    public RecordIdentifier(string value) 
        : base(value)
    {
    }
}
