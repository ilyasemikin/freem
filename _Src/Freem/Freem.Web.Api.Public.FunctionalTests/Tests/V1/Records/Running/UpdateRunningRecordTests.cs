using System.Net;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Records.Running;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running;

public sealed class UpdateRunningRecordTests : RunningRecordTestBase
{
    public UpdateRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        var request = new StartRunningRecordRequest(null, AddedRelatedActivities);
        var response = Context.SyncClient.Records.Start(request);
        response.EnsureSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var name = new RecordName("new_name");
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };

        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateDescription()
    {
        var description = new RecordDescription("new_description");
        var request = new UpdateRunningRecordRequest
        {
            Description = new UpdateField<RecordDescription?>(description)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateActivities()
    {
        var activityId = Context.Preparer.Activities.Create("name");

        var activities = new RelatedActivitiesIdentifiersCollection(activityId);
        var request = new UpdateRunningRecordRequest
        {
            Activities = new UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>>(activities)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
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
        var request = new UpdateRunningRecordRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>(tags)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateRunningRecordRequest();
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRunningRecordNotExists()
    {
        var srq = new StopRunningRecordRequest();
        var srs = await Context.Client.Records.StopAsync(srq);
        srs.EnsureSuccess();
        
        var name = new RecordName("new_name");
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}