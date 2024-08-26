using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class TagEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public TagEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task TagEvent_ShouldSuccess_WhenTagExists()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var tag = factory.CreateTag();

        await Context.Users.AddAsync(user);
        await Context.Tags.AddAsync(tag);

        var @event = new TagEventEntity
        {
            Id = "id",
            TagId = tag.Id,
            UserId = user.Id,
            Action = EventAction.Created,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
}