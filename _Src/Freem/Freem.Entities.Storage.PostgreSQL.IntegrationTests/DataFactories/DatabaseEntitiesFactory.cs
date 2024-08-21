using AutoFixture;
using Freem.AutoFixture.SpecimenBuilders;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;

internal sealed class DatabaseEntitiesFactory
{
    private readonly Fixture _fixture;

    public UserEntity User { get; }

    private DatabaseEntitiesFactory(string userId)
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());

        _fixture.Customize<TagEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Categories)
            .Without(e => e.Records)
            .Without(e => e.RunningRecords));

        _fixture.Customize<CategoryEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Tags));

        _fixture.Customize<RecordEntity>(builder => builder
            .With(e => e.UserId, userId)
            .With(e => e.StartAt, DateTimeOffset.UtcNow - TimeSpan.FromHours(10))
            .With(e => e.EndAt, DateTimeOffset.UtcNow)
            .Without(e => e.User)
            .Without(e => e.Categories)
            .Without(e => e.Tags));

        _fixture.Customize<RunningRecordEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Categories)
            .Without(e => e.Tags));

        User = _fixture
            .Build<UserEntity>()
            .With(e => e.Id, userId)
            .Without(e => e.DeletedAt)
            .Create();
    }

    public RunningRecordEntity CreateRunningRecord()
    {
        return _fixture.Create<RunningRecordEntity>();
    }

    public IReadOnlyList<TagEntity> CreateTags(int count)
    {
        return _fixture
            .CreateMany<TagEntity>(count)
            .ToArray();
    }

    public TagEntity CreateTag()
    {
        return _fixture.Create<TagEntity>();
    }

    public IReadOnlyList<CategoryEntity> CreateCategories(int count)
    {
        return _fixture
            .CreateMany<CategoryEntity>(count)
            .ToArray();
    }

    public CategoryEntity CreateCategory()
    {
        return _fixture.Create<CategoryEntity>();
    }

    public IReadOnlyList<RecordEntity> CreateRecords(int count)
    {
        return _fixture
            .CreateMany<RecordEntity>(count)
            .ToArray();
    }

    public RecordEntity CreateRecord()
    {
        return _fixture.Create<RecordEntity>();
    }

    public static DatabaseEntitiesFactory CreateFirstUserEntitiesFactory() => new("user1");
    public static DatabaseEntitiesFactory CreateSecondUserEntitiesFactory() => new("user2");
}