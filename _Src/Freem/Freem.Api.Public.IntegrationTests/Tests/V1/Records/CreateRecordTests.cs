using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class CreateRecordTests : RecordTestBase
{
    public CreateRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var startAt = DateTimeOffset.UtcNow - TimeSpan.FromHours(1);
        var endAt = startAt + TimeSpan.FromHours(1);
        var period = new DateTimePeriod(startAt, endAt);
        
        var request = new CreateRecordRequest(period, AddedRelatedActivities, AddedRelatedTags);
        
        var response = await Context.Client.Records.CreateAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}