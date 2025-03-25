using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Mocks;

internal class SampleIdentifier : StringIdentifier, IEntityIdentifier
{
    public SampleIdentifier() 
        : base("123")
    {
    }
    
    public bool Equals(IEntityIdentifier? other)
    {
        throw new NotImplementedException("SampleIdentifier for sample purpose only");
    }
}