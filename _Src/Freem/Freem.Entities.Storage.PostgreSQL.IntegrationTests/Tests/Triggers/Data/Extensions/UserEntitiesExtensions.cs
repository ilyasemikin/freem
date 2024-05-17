using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data.Extensions;

internal static class UserEntitiesExtensions
{
    public static async Task AddUserAsync(this UserEntities entities, DatabaseContext context)
    {
        await context.Users.AddAsync(entities.User);
    }

    public static async Task AddCategoriesAsync(this UserEntities entities, DatabaseContext context)
    {
        await context.Categories.AddRangeAsync(entities.Categories);
    }

    public static async Task AddRecordsAsync(this UserEntities entities, DatabaseContext context)
    {
        await context.Records.AddRangeAsync(entities.Records);
    }

    public static async Task AddRunningRecordAsync(this UserEntities entities, DatabaseContext context)
    {
        await context.RunningRecords.AddRangeAsync(entities.RunningRecord);
    }

    public static async Task AddTagsAsync(this UserEntities entities, DatabaseContext context)
    {
        await context.Tags.AddRangeAsync(entities.Tags);
    }

    public static async Task AddEntities(this UserEntities entities, DatabaseContext context)
    {
        await entities.AddUserAsync(context);
        await entities.AddCategoriesAsync(context);
        await entities.AddRecordsAsync(context);
        await entities.AddRunningRecordAsync(context);
        await entities.AddTagsAsync(context);
    }
}
