using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class RunningRecordEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RunningRecordEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    public async Task RunningRecordEvent_ShouldSuccess_WhenRunningRecordExists()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var record = factory.CreateRunningRecord();

        await Context.Users.AddAsync(user);
        await Context.RunningRecords.AddAsync(record);

        var @event = new RunningRecordEventEntity
        {
            Id = "id",
            UserId = user.Id,
            Action = EventAction.Created,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
}