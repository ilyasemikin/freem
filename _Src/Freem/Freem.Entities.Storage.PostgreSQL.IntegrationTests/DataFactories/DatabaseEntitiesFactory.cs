﻿using AutoFixture;
using Freem.AutoFixture.SpecimenBuilders;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Time;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;

internal sealed class DatabaseEntitiesFactory
{
    private readonly Fixture _fixture;

    public UserEntity User { get; }

    internal DatabaseEntitiesFactory(string userId)
    {
        _fixture = new Fixture();
        _fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());

        var utc = DateTimeOperations.EraseMilliseconds(DateTimeOffset.UtcNow);

        _fixture.Customize<TagEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Activities)
            .Without(e => e.Records)
            .Without(e => e.RunningRecords)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<ActivityEntity>(builder => builder
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Tags)
            .Without(e => e.UpdatedAt));

        _fixture.Customize<RecordEntity>(builder => builder
            .With(e => e.UserId, userId)
            .With(e => e.StartAt, utc.AddHours(-10))
            .With(e => e.EndAt, utc)
            .Without(e => e.User)
            .Without(e => e.Activities)
            .Without(e => e.Tags)
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
            .Without(e => e.PasswordCredentials)
            .Without(e => e.TelegramIntegration)
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