using AutoFixture;
using Freem.AutoFixture.SpecimenBuilders;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants.EntitiesNames;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data;

internal sealed class UserEntities
{
    public const int ManyEntitiesCount = 2;

    public UserEntity User { get; }

    public IReadOnlyList<CategoryEntity> Categories { get; }
    public IReadOnlyList<RecordEntity> Records { get; }
    public RunningRecordEntity RunningRecord { get; }

    public IReadOnlyList<TagEntity> Tags { get; }

    private UserEntities(string userId)
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());

        User = fixture
            .Build<UserEntity>()
            .With(e => e.Id, userId)
            .Without(e => e.DeletedAt)
            .Create();

        Categories = fixture
            .Build<CategoryEntity>()
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Tags)
            .CreateMany(ManyEntitiesCount)
            .ToArray();

        Records = CreateRecords(fixture, userId, Categories)
            .ToArray();

        RunningRecord = fixture
            .Build<RunningRecordEntity>()
            .With(e => e.UserId, userId)
            .With(e => e.Categories, Categories.ToArray())
            .Without(e => e.Tags)
            .Without(e => e.User)
            .Create();

        Tags = fixture
            .Build<TagEntity>()
            .With(e => e.UserId, userId)
            .Without(e => e.User)
            .Without(e => e.Categories)
            .Without(e => e.Records)
            .Without(e => e.RunningRecords)
            .CreateMany(ManyEntitiesCount)
            .ToArray();
    }

    private static IEnumerable<RecordEntity> CreateRecords(IFixture fixture, string userId, IReadOnlyList<CategoryEntity> categories)
    {
        var records = fixture
            .Build<RecordEntity>()
            .With(e => e.UserId, userId)
            .With(e => e.Categories, categories.ToArray())
            .Without(e => e.User)
            .Without(e => e.Tags)
            .CreateMany(ManyEntitiesCount);

        foreach (var entity in records)
        {
            if (entity.StartAt > entity.EndAt)
                (entity.StartAt, entity.EndAt) = (entity.EndAt, entity.StartAt);

            yield return entity;
        }
    }

    public static UserEntities CreateFirstUserEntities() => new UserEntities("user1");
    public static UserEntities CreateSecondUserEntities() => new UserEntities("user2");
}