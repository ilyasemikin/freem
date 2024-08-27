using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class RecordEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RecordEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RecordEvent_ShouldSuccess_WhenRecordExists()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();
        var record = factory.CreateRecord();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);
        await Context.Records.AddAsync(record);

        var relation = new RecordCategoryRelationEntity
        {
            RecordId = record.Id,
            CategoryId = category.Id
        };

        await Context.AddAsync(relation);
        
        var @event = new RecordEventEntity
        {
            Id = "id",
            RecordId = record.Id,
            UserId = user.Id,
            Action = EventAction.Created,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
}