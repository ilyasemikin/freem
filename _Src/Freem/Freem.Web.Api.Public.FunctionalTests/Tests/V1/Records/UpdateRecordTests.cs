using System.Net;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records;

public sealed class UpdateRecordTests : RecordTestBase
{
    public UpdateRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var name = new RecordName("name");
        var request = new UpdateRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };

        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateDescription()
    {
        var description = new RecordDescription("description");
        var request = new UpdateRecordRequest
        {
            Description = new UpdateField<RecordDescription?>(description)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateActivities()
    {
        var activityId = Context.Preparer.Activities.Create("name");
        var activities = new RelatedActivitiesIdentifiersCollection(activityId);

        var request = new UpdateRecordRequest
        {
            Activities = new UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>>(activities)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateTags()
    {
        var trq = new CreateTagRequest("name");
        var trs = await Context.Client.Tags.CreateAsync(trq);
        trs.EnsureSuccess();
        
        var tags = new RelatedTagsCollection([trs.Value.Id]);

        var request = new UpdateRecordRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>(tags)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateRecordRequest();
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        var idString = Guid.NewGuid().ToString();
        var id = new RecordIdentifier(idString);
        
        var name = new RecordName("name");
        var request = new UpdateRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateAsync(id, request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}