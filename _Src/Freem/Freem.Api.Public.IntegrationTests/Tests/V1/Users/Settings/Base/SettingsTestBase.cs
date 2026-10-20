using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Users.Settings.Base;

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