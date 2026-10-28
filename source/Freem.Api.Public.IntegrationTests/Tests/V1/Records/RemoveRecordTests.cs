using System.Net;
using Freem.Domain.Records;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class RemoveRecordTests : RecordTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Records.RemoveAsync(AddedRecordIds[0]);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        var id = RecordId.Generate();
        
        var response = await Context.Client.Records.RemoveAsync(id);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}