using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers;

public sealed class CategoriesTagsConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    [Fact]
    public async Task CategoriesTags_ShouldExecute_WhenUserIdsCorrect()
    {
        var entities = UserEntities.CreateFirstUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);
        await entities.AddTagsAsync(Context);

        var relation = new CategoryTagRelationEntity
        {
            CategoryId = entities.Categories[0].Id,
            TagId = entities.Tags[0].Id
        };

        Context.Add(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task CategoriesTags_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var first = UserEntities.CreateFirstUserEntities();
        var second = UserEntities.CreateSecondUserEntities();

        await first.AddUserAsync(Context);
        await first.AddCategoriesAsync(Context);

        await second.AddUserAsync(Context);
        await second.AddTagsAsync(Context);

        var relation = new CategoryTagRelationEntity
        {
            CategoryId = first.Categories[0].Id,
            TagId = second.Tags[0].Id
        };

        Context.Add(relation);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must be equils"));
    }
}
