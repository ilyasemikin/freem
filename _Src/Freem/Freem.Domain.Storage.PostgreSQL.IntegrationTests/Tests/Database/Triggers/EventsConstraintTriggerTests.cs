using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class EventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    private static readonly DateTimeOffset OccurredAt = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    public EventsConstraintTriggerTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task Events_ShouldThrowException_WhenUserIsNotExists()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.AddRangeAsync(user, activity);

        var entity = new EventEntity
        {
            Id = "id",
            UserId = "not_existed_user_id",
            EventType = "activities.created",
            EntityType = EntityNames.Activity,
            EntityRef = activity.Id,
            OccurredAt = OccurredAt,
            Payload = "{}"
        };

        await Context.Events.AddAsync(entity);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.EventsRelatedEntityNotExists));
    }

    [Theory]
    [InlineData(EntityNames.Activity, "activities.created")]
    [InlineData(EntityNames.Record, "records.created")]
    [InlineData(EntityNames.RunningRecord, "running_records.started")]
    [InlineData(EntityNames.Tag, "tags.created")]
    [InlineData(EntityNames.User, "users.registered")]
    public async Task Events_ShouldThrowException_WhenRelatedEntityNotExists(string name, string eventType)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.AddAsync(user);

        var entity = new EventEntity
        {
            Id = "id",
            UserId = user.Id,
            EventType = eventType,
            EntityType = name,
            EntityRef = "not_existed_id",
            OccurredAt = OccurredAt,
            Payload = "{}"
        };

        await Context.Events.AddAsync(entity);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e =>
                e.Message.Contains(TriggerErrorCodes.EventsRelatedEntityNotExists) &&
                e.Message.Contains(name) &&
                e.Message.Contains(entity.EntityRef));
    }

    [Theory]
    [InlineData(EntityNames.Activity, "activities.removed")]
    [InlineData(EntityNames.Record, "records.removed")]
    [InlineData(EntityNames.RunningRecord, "running_records.stopped")]
    [InlineData(EntityNames.Tag, "tags.removed")]
    public async Task Events_ShouldNotThrowException_WhenEntityIsRemovedAndNotExists(string name, string eventType)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.AddAsync(user);

        var entity = new EventEntity
        {
            Id = "id",
            UserId = user.Id,
            EventType = eventType,
            EntityType = name,
            EntityRef = "not_existed_id",
            OccurredAt = OccurredAt,
            Payload = "{}"
        };

        await Context.Events.AddAsync(entity);

        var exception = await Record.ExceptionAsync(() => Context.SaveChangesAsync());

        Assert.Null(exception);
    }
}
