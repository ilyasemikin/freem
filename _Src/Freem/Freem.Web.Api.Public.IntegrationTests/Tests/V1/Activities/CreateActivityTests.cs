using Freem.Domain.Activities.Models;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class CreateActivityTests : ActivityTestBase
{
    public CreateActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = ActivityName.Parse("name", null);
        var request = new CreateActivityRequest(name, AddedRelatedTags);
        
        var response = await Context.Client.Activities.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}