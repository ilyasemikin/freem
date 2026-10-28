using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Domain.Activities.Models;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Activities.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class CreateActivityTests : ActivityTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = ActivityName.Parse("name", null);
        var color = ActivityColor.Parse("#10b981", null);
        var request = new ActivityCreateRequest(name, color, AddedRelatedTags);
        
        var response = await Context.Client.Activities.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}