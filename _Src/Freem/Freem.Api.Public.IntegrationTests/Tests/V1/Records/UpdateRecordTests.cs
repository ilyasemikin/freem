using System.Net;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class UpdateRecordTests : RecordTestBase
{
    public UpdateRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var name = RecordName.Parse("name", null);
        var request = new RecordUpdateRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };

        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateDescription()
    {
        var description = RecordDescription.Parse("description", null);
        var request = new RecordUpdateRequest
        {
            Description = new UpdateField<RecordDescription?>(description)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateActivities()
    {
        var activityId = await Context.Preparer.Activities.CreateAsync("name");
        var activities = new RelatedActivityIdsCollection(activityId);

        var request = new RecordUpdateRequest
        {
            Activities = new UpdateField<IReadOnlyRelatedIdsCollection<ActivityId>>(activities)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
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

        var request = new RecordUpdateRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedIdsCollection<TagId>>(tags)
        };
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new RecordUpdateRequest();
        
        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        var idString = Guid.NewGuid().ToString();
        var id = RecordId.Parse(idString, null);
        
        var name = RecordName.Parse("name", null);
        var request = new RecordUpdateRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateAsync(id, request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}