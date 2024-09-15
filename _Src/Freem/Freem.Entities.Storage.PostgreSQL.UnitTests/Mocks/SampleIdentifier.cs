using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Mocks;

internal class SampleIdentifier : IEntityIdentifier
{
    public string Value { get; } = nameof(SampleIdentifier);
    
    public bool Equals(IEntityIdentifier? other)
    {
        throw new NotImplementedException("SampleIdentifier for sample purpose only");
    }
}