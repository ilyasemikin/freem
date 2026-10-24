using System.Net;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class UpdateRunningRecordTests : RunningRecordTestBase
{
    public UpdateRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var request = new RunningRecordStartRequest(null, AddedRelatedActivities);
        var response = await Context.Client.Records.StartAsync(request);
        response.EnsureSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var name = RecordName.Parse("new_name", null);
        var request = new RunningRecordUpdateRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };

        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateDescription()
    {
        var description = RecordDescription.Parse("new_description", null);
        var request = new RunningRecordUpdateRequest
        {
            Description = new UpdateField<RecordDescription?>(description)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateActivities()
    {
        var activityId = await Context.Preparer.Activities.CreateAsync("name");

        var activities = new RelatedActivityIdsCollection(activityId);
        var request = new RunningRecordUpdateRequest
        {
            Activities = new UpdateField<IReadOnlyRelatedIdsCollection<ActivityId>>(activities)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateTags()
    {
        var tagName = TagName.Parse("name", null);
        var trq = new TagCreateRequest(tagName);
        var trs = await Context.Client.Tags.CreateAsync(trq);
        
        var tags = new RelatedTagIdsCollection([trs.Unwrap().Id]);
        var request = new RunningRecordUpdateRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedIdsCollection<TagId>>(tags)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new RunningRecordUpdateRequest();
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRunningRecordNotExists()
    {
        var srq = new RunningRecordStopRequest();
        var srs = await Context.Client.Records.StopAsync(srq);
        srs.EnsureSuccess();
        
        var name = RecordName.Parse("new_name", null);
        var request = new RunningRecordUpdateRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateRunningAsync(request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}