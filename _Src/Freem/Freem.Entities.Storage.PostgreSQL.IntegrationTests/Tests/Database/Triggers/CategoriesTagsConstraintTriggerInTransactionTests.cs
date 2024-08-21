using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class CategoriesTagsConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public CategoriesTagsConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task CategoriesTags_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();
        var tag = factory.CreateTag();

        Context.Users.Add(user);
        Context.Categories.Add(category);
        Context.Tags.Add(tag);

        var relation = new CategoryTagRelationEntity
        {
            CategoryId = category.Id,
            TagId = tag.Id
        };

        Context.Add(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task CategoriesTags_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstCategory = firstFactory.CreateCategory();

        var secondUser = secondFactory.User;
        var secondTag = secondFactory.CreateTag();

        Context.Users.Add(firstUser);
        Context.Categories.Add(firstCategory);

        Context.Users.Add(secondUser);
        Context.Tags.Add(secondTag);

        var relation = new CategoryTagRelationEntity
        {
            CategoryId = firstCategory.Id,
            TagId = secondTag.Id
        };

        Context.Add(relation);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must be equils"));
    }
}
