using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Settings.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Settings;

public sealed class GetUserSettingsTests : SettingsTestBase
{
    public GetUserSettingsTests(TestContext context) 
        : base(context)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Users.GetSettingsAsync();
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }
}