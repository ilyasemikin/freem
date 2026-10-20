using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records;

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
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldApplyLimit()
    {
        var request = new ListRecordRequest(0, 2);

        var response = await Context.Client.Records.ListAsync(request);

        var records = response.Unwrap();
        Assert.Equal(2, records.Count);
    }

    [Fact]
    public async Task Request_ShouldApplyOffset()
    {
        var offset = AddedRecordIds.Count - 1;
        var request = new ListRecordRequest(offset, 100);

        var response = await Context.Client.Records.ListAsync(request);

        var records = response.Unwrap();
        Assert.Single(records);
    }
}