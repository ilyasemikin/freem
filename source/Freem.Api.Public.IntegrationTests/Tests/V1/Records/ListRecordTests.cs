using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class ListRecordTests : RecordTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new RecordListRequest(0, 100);

        var response = await Context.Client.Records.ListAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldApplyLimit()
    {
        var request = new RecordListRequest(0, 2);

        var response = await Context.Client.Records.ListAsync(request);

        var records = response.Unwrap().Records;
        Assert.Equal(2, records.Count);
    }

    [Fact]
    public async Task Request_ShouldApplyOffset()
    {
        var offset = AddedRecordIds.Count - 1;
        var request = new RecordListRequest(offset, 100);

        var response = await Context.Client.Records.ListAsync(request);

        var records = response.Unwrap().Records;
        Assert.Single(records);
    }
}