using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class ListByPeriodRecordTests : RecordTestBase
{
    public ListByPeriodRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var startAt = AddedPeriods.Min(p => p.StartAt);
        var endAt = AddedPeriods.Max(p => p.EndAt);

        var period = new DateTimePeriod(startAt, endAt);
        
        var request = new ListRecordByPeriodRequest(period, 100);

        var response = await Context.Client.Records.ListByPeriodAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}