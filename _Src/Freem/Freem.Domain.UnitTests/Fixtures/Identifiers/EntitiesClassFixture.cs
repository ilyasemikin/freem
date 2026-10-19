using AutoFixture;
using Freem.Domain.UnitTests.Mocs.Entities;

namespace Freem.Domain.UnitTests.Fixtures.Identifiers;

public sealed class EntitiesClassFixture
{
    private readonly Fixture _fixture;

    public EntitiesClassFixture()
    {
        _fixture = new Fixture();
    }

    public IReadOnlyList<TestIdentifier> CreateIdentifiers(int count)
    {
        return _fixture
            .CreateMany<TestIdentifier>(count)
            .ToArray();
    }

    public TestIdentifier CreateIdentifier()
    {
        return _fixture.Create<TestIdentifier>();
    }
}