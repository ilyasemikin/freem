using Freem.Entities.Storage.PostgreSQL.Database.Constants;
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

public sealed class RecordsTagsConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public RecordsTagsConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RecordsTags_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var record = factory.CreateRecord();
        var tag = factory.CreateTag();

        await Context.Users.AddAsync(user);
        await Context.Records.AddAsync(record);
        await Context.Tags.AddAsync(tag);

        var relation = new RecordTagRelationEntity
        {
            RecordId = record.Id,
            TagId = tag.Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RecordsTags_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstRecord = firstFactory.CreateRecord();

        var secondUser = secondFactory.User;
        var secondTag = secondFactory.CreateTag();

        await Context.Users.AddAsync(firstUser);
        await Context.Records.AddAsync(firstRecord);

        await Context.Users.AddAsync(secondUser);
        await Context.Tags.AddAsync(secondTag);

        var relation = new RecordTagRelationEntity
        {
            RecordId = firstRecord.Id,
            TagId = secondTag.Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsTagsDifferentUserIds));
    }
}
