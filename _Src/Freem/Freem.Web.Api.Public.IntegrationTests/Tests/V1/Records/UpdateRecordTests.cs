using System.Net;
using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Contracts.Models;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class UpdateRecordTests : RecordTestBase
{
    public UpdateRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdatePeriod()
    {
        var startAt = DateTimeOffset.UnixEpoch.AddYears(55);
        var period = new DateTimePeriod(startAt, startAt.AddHours(2));
        var request = new UpdateRecordRequest
        {
            Period = new UpdateField<DateTimePeriod>(period)
        };

        var response = await Context.Client.Records.UpdateAsync(AddedRecordIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();

        var actual = await Context.Client.Records.GetAsync(AddedRecordIds[0]);
        
        Assert.NotNull(actual);
        Assert.NotNull(actual.Value);
        Assert.Equal(period.StartAt, actual.Value.Period.StartAt);
        Assert.Equal(period.EndAt, actual.Value.Period.EndAt);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var name = RecordName.Parse("name", null);
        var request = new UpdateRecordRequest
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
        var request = new UpdateRecordRequest
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

        var request = new UpdateRecordRequest
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
        var trq = new CreateTagRequest(tagName);
        var trs = await Context.Client.Tags.CreateAsync(trq);
        
        var tags = new RelatedTagIdsCollection([trs.Unwrap().Id]);

        var request = new UpdateRecordRequest
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
        var request = new UpdateRecordRequest();
        
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
        var request = new UpdateRecordRequest
        {
            Name = new UpdateField<RecordName?>(name)
        };
        
        var response = await Context.Client.Records.UpdateAsync(id, request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}