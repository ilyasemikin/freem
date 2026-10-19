using System.Net;
using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Contracts.Models;
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
        var name = RecordName.Parse("new_name", null);
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
        var description = RecordDescription.Parse("new_description", null);
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

        var activities = new RelatedActivityIdsCollection(activityId);
        var request = new UpdateRunningRecordRequest
        {
            Activities = new UpdateField<IReadOnlyRelatedIdsCollection<ActivityId>>(activities)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateTags()
    {
        var tagName = TagName.Parse("name", null);
        var trq = new CreateTagRequest(tagName);
        var trs = await Context.Client.Tags.CreateAsync(trq);
        trs.EnsureSuccess();
        
        var tags = new RelatedTagIdsCollection([trs.Value.Id]);
        var request = new UpdateRunningRecordRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedIdsCollection<TagId>>(tags)
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
        
        var name = RecordName.Parse("new_name", null);
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}