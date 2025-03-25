using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Events.Base;

public abstract class EventTestBase : TestBase
{
    public EventTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();
        Context.Preparer.Users.Login();
    }
}