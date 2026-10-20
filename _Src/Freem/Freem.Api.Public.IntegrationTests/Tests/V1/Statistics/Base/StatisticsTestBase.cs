using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Statistics.Base;

public abstract class StatisticsTestBase : TestBase
{
    public StatisticsTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }
}