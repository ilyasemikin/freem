using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Freem.Time;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

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
        
        var request = new RecordListByPeriodRequest(period, 100);

        var response = await Context.Client.Records.ListByPeriodAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}