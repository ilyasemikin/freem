using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records;

public sealed class GetRecordTests : RecordTestBase
{
    public GetRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Records.GetAsync(AddedRecordIds[0]);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        var id = Guid.NewGuid().ToString();
        
        var response = await Context.Client.Records.GetAsync(id);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Value);
    }
}