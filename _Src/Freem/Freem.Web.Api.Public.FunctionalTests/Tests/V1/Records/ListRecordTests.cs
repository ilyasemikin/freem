using System.Net;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records;

public sealed class ListRecordTests : RecordTestBase
{
    public ListRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new ListRecordRequest(0, 100);

        var response = await Context.Client.Records.ListAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }
}