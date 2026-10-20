using System.Net;
using Freem.Domain.Records;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records;

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
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        var id = RecordId.Generate();
        
        var response = await Context.Client.Records.GetAsync(id);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        Assert.Null(response.Value);
    }
}