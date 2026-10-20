using System.Net;
using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Api.Public.Contracts.DTO.Activities;
using Freem.Api.Public.Contracts.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class UpdateActivityTests : ActivityTestBase
{
    public UpdateActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateName()
    {
        var activityId = AddedActivityIds[0];
        var name = ActivityName.Parse("name", null);
        var request = new UpdateActivityRequest
        {
            Name = new UpdateField<ActivityName>(name)
        };

        var response = await Context.Client.Activities.UpdateAsync(activityId, request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateTags()
    {
        var tagId = await Context.Preparer.Tags.CreateAsync("name");
        var tags = new RelatedTagIdsCollection(tagId);
        var request = new UpdateActivityRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedIdsCollection<TagId>>(tags)
        };
        
        var response = await Context.Client.Activities.UpdateAsync(AddedActivityIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityDoesNotExist()
    {
        var activityId = ActivityId.Generate();
        var name = ActivityName.Parse("name", null);
        var request = new UpdateActivityRequest
        {
            Name = new UpdateField<ActivityName>(name)
        };
        
        var response = await Context.Client.Activities.UpdateAsync(activityId, request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateActivityRequest();
        
        var response = await Context.Client.Activities.UpdateAsync(AddedActivityIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }
}