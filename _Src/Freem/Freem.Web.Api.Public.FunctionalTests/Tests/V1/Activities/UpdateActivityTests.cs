using System.Net;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.Contracts.Models;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities;

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
        var name = new ActivityName("name");
        var request = new UpdateActivityRequest
        {
            Name = new UpdateField<ActivityName>(name)
        };

        var response = await Context.Client.Activities.UpdateAsync(activityId, request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenUpdateTags()
    {
        var tagId = Context.Preparer.Tags.Create("name");
        var tags = new RelatedTagsIdentifiersCollection(tagId);
        var request = new UpdateActivityRequest
        {
            Tags = new UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>(tags)
        };
        
        var response = await Context.Client.Activities.UpdateAsync(AddedActivityIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityDoesNotExist()
    {
        var activityId = Guid.NewGuid().ToString();
        var name = new ActivityName("name");
        var request = new UpdateActivityRequest
        {
            Name = new UpdateField<ActivityName>(name)
        };
        
        var response = await Context.Client.Activities.UpdateAsync(activityId, request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateActivityRequest();
        
        var response = await Context.Client.Activities.UpdateAsync(AddedActivityIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}