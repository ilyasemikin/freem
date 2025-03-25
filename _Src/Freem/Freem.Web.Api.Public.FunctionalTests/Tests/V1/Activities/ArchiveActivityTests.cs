using System.Net;
using Freem.Entities.Activities.Identifiers;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities;

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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityHasInvalidStatus()
    {
        var id = AddedActivityIds[0];
        Context.Preparer.Activities.Archive(id);
        
        var response = await Context.Client.Activities.ArchiveAsync(id);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityNotExists()
    {
        var idString = Guid.NewGuid().ToString();
        var id = new ActivityIdentifier(idString);
        
        var response = await Context.Client.Activities.ArchiveAsync(id);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}