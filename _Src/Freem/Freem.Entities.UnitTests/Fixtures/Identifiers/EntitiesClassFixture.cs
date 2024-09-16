using AutoFixture;
using Freem.Entities.UnitTests.Mocs.Entities;

namespace Freem.Entities.UnitTests.Fixtures.Identifiers;

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
    
    public IReadOnlyList<TestEntity> CreateEntities(int count)
    {
        return _fixture
            .CreateMany<TestEntity>(count)
            .ToArray();
    }

    public TestIdentifier CreateIdentifier()
    {
        return _fixture.Create<TestIdentifier>();
    }

    public TestEntity CreateEntity()
    {
        return _fixture.Create<TestEntity>();
    }
}