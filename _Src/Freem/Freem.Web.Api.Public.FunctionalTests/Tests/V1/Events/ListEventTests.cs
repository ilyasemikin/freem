using System.Net;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Events.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Events;

public sealed class ListEventTests : EventTestBase
{
    public ListEventTests(TestContext context, ITestOutputHelper? output = null) : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new ListEventRequest(100);

        var response = await Context.Client.Events.ListAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}