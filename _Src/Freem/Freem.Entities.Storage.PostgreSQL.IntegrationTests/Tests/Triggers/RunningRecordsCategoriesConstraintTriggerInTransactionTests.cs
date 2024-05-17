using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers;

public sealed class RunningRecordsCategoriesConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    [Fact]
    public async Task RunningRecordsCategories_ShouldExecute_WhenUserIdsCorrect()
    {
        var entities = UserEntities.CreateFirstUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);

        var record = entities.RunningRecord;
        record.Categories = null;

        await Context.RunningRecords.AddAsync(record);

        var relation = new RunningRecordCategoryRelationEntity
        {
            RunningRecordId = record.UserId,
            CategoryId = entities.Categories[0].Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var first = UserEntities.CreateFirstUserEntities();
        var second = UserEntities.CreateSecondUserEntities();

        await first.AddUserAsync(Context);
        await first.AddCategoriesAsync(Context);

        await second.AddUserAsync(Context);
        await second.AddCategoriesAsync(Context);

        var record = first.RunningRecord;
        record.Categories = null;

        await Context.RunningRecords.AddAsync(record);

        var relations = new[]
        {
            new RunningRecordCategoryRelationEntity
            {
                RunningRecordId = record.UserId,
                CategoryId = first.Categories[0].Id
            },
            new RunningRecordCategoryRelationEntity
            {
                RunningRecordId = record.UserId,
                CategoryId = second.Categories[0].Id
            }
        };

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<DbUpdateException>();
    }
}
