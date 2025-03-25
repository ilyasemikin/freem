using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Settings.Base;

public abstract class SettingsTestBase : TestBase
{
    protected SettingsTestBase(TestContext context) 
        : base(context)
    {
        Context.Preparer.Users.Register();
        Context.Preparer.Users.Login();
    }
}