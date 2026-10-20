using System.Net;
using Freem.Domain.Activities;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class ArchiveActivityTests : ActivityTestBase
{
    public ArchiveActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Activities.ArchiveAsync(AddedActivityIds[0]);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityHasInvalidStatus()
    {
        var id = AddedActivityIds[0];
        await Context.Preparer.Activities.ArchiveAsync(id);
        
        var response = await Context.Client.Activities.ArchiveAsync(id);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityNotExists()
    {
        var idString = Guid.NewGuid().ToString();
        var id = ActivityId.Parse(idString, null);
        
        var response = await Context.Client.Activities.ArchiveAsync(id);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}