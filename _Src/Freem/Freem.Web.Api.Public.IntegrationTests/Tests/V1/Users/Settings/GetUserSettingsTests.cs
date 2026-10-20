using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Users.Settings.Base;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Users.Settings;

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
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}