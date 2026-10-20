using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Events.Base;

public abstract class EventTestBase : TestBase
{
    public EventTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }
}