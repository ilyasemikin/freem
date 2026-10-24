using AutoFixture;
using Freem.Domain.UnitTests.Mocs.Entities;

namespace Freem.Domain.UnitTests.Fixtures.Ids;

public sealed class EntitiesClassFixture
{
    private readonly Fixture _fixture;

    public EntitiesClassFixture()
    {
        _fixture = new Fixture();
    }

    public IReadOnlyList<TestId> CreateIds(int count)
    {
        return _fixture
            .CreateMany<TestId>(count)
            .ToArray();
    }

    public TestId CreateId()
    {
        return _fixture.Create<TestId>();
    }
}