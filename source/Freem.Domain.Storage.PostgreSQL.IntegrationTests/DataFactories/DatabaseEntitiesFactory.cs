using AutoFixture;
using Freem.Domain.Records.Sources;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories.SpecimenBuilders;
using Freem.Time;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;

internal sealed class DatabaseEntitiesFactory
{
    private readonly Fixture _fixture;

    public UserEntity User { get; }

    private readonly DateTimeOffset _now = DateTimeOperations.TruncateToMilliseconds(DateTimeOffset.UtcNow);

    private static RecordPeriodEntity CreatePeriod(string recordId, DateTimeOffset startAt, DateTimeOffset endAt)
    {
        return new RecordPeriodEntity
        {
            Id = Guid.NewGuid().ToString("N"),
            RecordId = recordId,
            StartAt = startAt,
            EndAt = endAt
        };
    }

    internal DatabaseEntitiesFactory(string userId)
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());

        var utc = DateTimeOperations.TruncateToMilliseconds(DateTimeOffset.UtcNow);

        _fixture.Customize<TagEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Activities)
            .Without(e => e.Records)
            .Without(e => e.RunningRecords)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<ActivityEntity>(builder => builder
            .With(e => e.UserId, userId)
            .With(e => e.Color, "#6366f1")
            .Without(e => e.User)
            .Without(e => e.Tags)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<RecordEntity>(builder => builder
            .With(e => e.UserId, userId)
            .With(e => e.SourceType, RecordSourceType.Manual.ToString("G"))
            .Without(e => e.SourceImportDocumentId)
            .Without(e => e.SourceImportRowId)
            .Without(e => e.SourceImportFormat)
            .Without(e => e.SourceImportRecordId)
            .Without(e => e.User)
            .Without(e => e.Periods)
            .Without(e => e.Activities)
            .Without(e => e.Tags)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<RecordPeriodEntity>(builder => builder
            .Without(e => e.Record)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<RunningRecordEntity>(builder => builder
            .With(e => e.UserId, userId)
            .With(e => e.StartAt, utc)
            .Without(e => e.User)
            .Without(e => e.Activities)
            .Without(e => e.Tags)
            .Without(e => e.UpdatedAt));

        User = _fixture
            .Build<UserEntity>()
            .With(e => e.Id, userId)
            .Without(e => e.DeletedAt)
            .Without(e => e.UpdatedAt)
            .Without(e => e.Settings)
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

    public IReadOnlyList<ActivityEntity> CreateActivities(int count)
    {
        return _fixture
            .CreateMany<ActivityEntity>(count)
            .ToArray();
    }

    public ActivityEntity CreateActivity()
    {
        return _fixture.Create<ActivityEntity>();
    }

    public IReadOnlyList<RecordEntity> CreateRecords(int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => CreateRecord())
            .ToArray();
    }

    public RecordEntity CreateRecord()
    {
        var record = _fixture.Create<RecordEntity>();
        record.Periods = [CreatePeriod(record.Id, _now.AddHours(-10), _now)];
        return record;
    }

    public RecordEntity CreateImportRecord()
    {
        var record = CreateRecord();
        return new RecordEntity
        {
            Id = record.Id,
            UserId = record.UserId,
            Name = record.Name,
            Description = record.Description,
            Periods = record.Periods,
            CreatedAt = record.CreatedAt,
            SourceType = RecordSourceType.Import.ToString("G"),
            SourceImportDocumentId = _fixture.Create<string>(),
            SourceImportRowId = _fixture.Create<string>(),
            SourceImportFormat = "time-tracker",
            SourceImportRecordId = _fixture.Create<string>()
        };
    }

    public static DatabaseEntitiesFactory CreateFirstUserEntitiesFactory() => new("user1");
    public static DatabaseEntitiesFactory CreateSecondUserEntitiesFactory() => new("user2");
}