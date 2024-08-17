using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class RecordsCategoriesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RecordsCategoriesConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RecordsCategories_ShouldThrowException_WhenNoCategoriesAdded()
    {
        var factory = EntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var record = factory.CreateRecord();
        record.Categories = null;

        await Context.Records.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have categories count in range"));
    }

    [Fact]
    public async Task RecordsCategories_ShouldThrowException_WhenDeleteLastCategory()
    {
        var factory = EntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var record = factory.CreateRecord();
        record.Categories = new List<CategoryEntity>() { category };

        await Context.Records.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.Records
            .Include(e => e.Categories)
            .FirstAsync(e => e.Id == record.Id);
        record.Categories = null;

        Context.Records.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have categories count in range [1, 16]"));
    }

    [Fact]
    public async Task RecordCategories_ShouldThrowException_WhenAddToManyTags()
    {
        var factory = EntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var record = factory.CreateRecord();
        var categories = factory.CreateCategories(RelatedCategoriesCollection.MaxCategoriesCount + 1);

        record.Categories = new List<CategoryEntity>() { categories[0] };

        await Context.Users.AddAsync(user);
        await Context.Records.AddAsync(record);
        await Context.Categories.AddRangeAsync(categories);

        await Context.SaveChangesAsync();

        var relations = categories
            .Skip(1)
            .Select(e => new RecordCategoryRelationEntity
            {
                RecordId = record.Id,
                CategoryId = e.Id
            });

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have categories count in range [1, 16]"));
    }
}
