using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class ActivitiesTagsConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public ActivitiesTagsConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task ActivitiesTags_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();
        var tag = factory.CreateTag();

        Context.Users.Add(user);
        Context.Activities.Add(activity);
        Context.Tags.Add(tag);

        var relation = new ActivityTagRelationEntity
        {
            ActivityId = activity.Id,
            TagId = tag.Id
        };

        Context.Add(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task ActivitiesTags_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstActivity = firstFactory.CreateActivity();

        var secondUser = secondFactory.User;
        var secondTag = secondFactory.CreateTag();

        Context.Users.Add(firstUser);
        Context.Activities.Add(firstActivity);

        Context.Users.Add(secondUser);
        Context.Tags.Add(secondTag);

        var relation = new ActivityTagRelationEntity
        {
            ActivityId = firstActivity.Id,
            TagId = secondTag.Id
        };

        Context.Add(relation);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.ActivitiesTagsDifferentUserIds));
    }
}
