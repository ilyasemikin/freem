using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Users.Settings.Base;

public abstract class SettingsTestBase : TestBase
{
    protected SettingsTestBase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }
}