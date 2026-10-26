using System.Net;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Records.Sources;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Freem.Time;
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
        
        var request = new RecordCreateRequest([new RecordPeriodItem(period)], AddedRelatedActivities, AddedRelatedTags);
        
        var response = await Context.Client.Records.CreateAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldReturnManualSource()
    {
        var startAt = DateTimeOffset.UtcNow - TimeSpan.FromHours(1);
        var period = new DateTimePeriod(startAt, TimeSpan.FromHours(1));
        var request = new RecordCreateRequest([new RecordPeriodItem(period)], AddedRelatedActivities, AddedRelatedTags);

        var created = (await Context.Client.Records.CreateAsync(request)).Unwrap();

        var actual = (await Context.Client.Records.GetAsync(created.Id)).Unwrap().Record;
        Assert.Equal(RecordSourceType.Manual, actual.Source.Type);
        Assert.Null(actual.Source.ImportId);
        Assert.Null(actual.Source.ImportRowId);
        Assert.Null(actual.Source.Format);
        Assert.Null(actual.Source.ImportRecordId);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenSeveralPeriods()
    {
        var startAt = DateTimeOffset.UtcNow - TimeSpan.FromHours(5);
        var description = RecordPeriodDescription.Parse("second", null);
        var periods = new[]
        {
            new RecordPeriodItem(new DateTimePeriod(startAt, startAt.AddHours(1))),
            new RecordPeriodItem(new DateTimePeriod(startAt.AddHours(2), startAt.AddHours(3))) { Description = description }
        };
        var request = new RecordCreateRequest(periods, AddedRelatedActivities, AddedRelatedTags);

        var response = await Context.Client.Records.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();

        var created = response.Unwrap();
        var actual = (await Context.Client.Records.GetAsync(created.Id)).Unwrap().Record;
        Assert.Equal(2, actual.Periods.Count);
        Assert.Equal(periods[1].Period, actual.Periods[1].Period);
        Assert.Equal(description, actual.Periods[1].Description);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNoPeriods()
    {
        var request = new RecordCreateRequest([], AddedRelatedActivities, AddedRelatedTags);

        var response = await Context.Client.Records.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenPeriodsOverlap()
    {
        var startAt = DateTimeOffset.UtcNow - TimeSpan.FromHours(5);
        var periods = new[]
        {
            new RecordPeriodItem(new DateTimePeriod(startAt, startAt.AddHours(2))),
            new RecordPeriodItem(new DateTimePeriod(startAt.AddHours(1), startAt.AddHours(3)))
        };
        var request = new RecordCreateRequest(periods, AddedRelatedActivities, AddedRelatedTags);

        var response = await Context.Client.Records.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }
}