using Freem.Api.Public.Contracts.V1.Records.Dtos;
using System.Net;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Users.Settings;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Freem.Time;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records;

public sealed class RecordPeriodTests : RecordTestBase
{
    public RecordPeriodTests(TestContext context, ITestOutputHelper? output = null)
        : base(context, output)
    {
    }

    private DateTimePeriod CreateAfterAddedPeriods(int startHours, int durationHours)
    {
        var startAt = AddedPeriods.Max(p => p.EndAt).AddHours(startHours);
        return new DateTimePeriod(startAt, startAt.AddHours(durationHours));
    }

    private async Task<RecordDto> GetRecordAsync(RecordId id)
    {
        var response = await Context.Client.Records.GetAsync(id);
        return response.Unwrap().Record;
    }

    [Fact]
    public async Task Add_ShouldReturnCurrentTimeZoneOffset()
    {
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));
        var settings = new UserSettingsUpdateRequest
        {
            TimeZoneOffset = new UpdateField<TimeZoneOffset>(offset)
        };
        var settingsResponse = await Context.Client.Users.UpdateSettingsAsync(settings);
        settingsResponse.ShouldSuccess();

        var period = CreateAfterAddedPeriods(1, 2);
        var body = new RecordPeriodItem(period);
        var response = await Context.Client.Records.AddPeriodAsync(AddedRecordIds[0], body);
        response.ShouldSuccess();

        var addedId = response.Unwrap().Id;
        var record = await GetRecordAsync(AddedRecordIds[0]);
        var added = Assert.Single(record.Periods, p => p.Id.Equals(addedId));
        Assert.Equal(offset, added.TimeZoneOffset);
        var existing = Assert.Single(record.Periods, p => !p.Id.Equals(addedId));
        Assert.Equal(TimeZoneOffset.Default, existing.TimeZoneOffset);
    }

    [Fact]
    public async Task Add_ShouldSuccess()
    {
        var period = CreateAfterAddedPeriods(1, 2);
        var description = RecordPeriodDescription.Parse("added", null);
        var body = new RecordPeriodItem(period) { Description = description };

        var response = await Context.Client.Records.AddPeriodAsync(AddedRecordIds[0], body);

        Assert.NotNull(response);
        response.ShouldSuccess();

        var addedId = response.Unwrap().Id;
        var record = await GetRecordAsync(AddedRecordIds[0]);
        Assert.Equal(2, record.Periods.Count);
        var added = Assert.Single(record.Periods, p => p.Id.Equals(addedId));
        Assert.Equal(period, added.Period);
        Assert.Equal(description, added.Description);
    }

    [Fact]
    public async Task Add_ShouldFail_WhenPeriodOverlaps()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);
        var existing = record.Periods[0].Period;
        var period = new DateTimePeriod(existing.StartAt, existing.EndAt.AddHours(1));

        var response = await Context.Client.Records.AddPeriodAsync(AddedRecordIds[0], new RecordPeriodItem(period));

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Add_ShouldFail_WhenRecordNotExists()
    {
        var body = new RecordPeriodItem(CreateAfterAddedPeriods(1, 1));

        var response = await Context.Client.Records.AddPeriodAsync(RecordId.Generate(), body);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldSuccess()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);
        var current = record.Periods[0];
        var period = new DateTimePeriod(current.Period.StartAt, current.Period.EndAt.AddMinutes(-1));
        var description = RecordPeriodDescription.Parse("changed", null);
        var body = new RecordPeriodUpdateRequest
        {
            Period = new UpdateField<DateTimePeriod>(period),
            Description = new UpdateField<RecordPeriodDescription?>(description)
        };

        var response = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], current.Id, body);

        Assert.NotNull(response);
        response.ShouldSuccess();

        var actual = (await GetRecordAsync(AddedRecordIds[0])).Periods[0];
        Assert.Equal(current.Id, actual.Id);
        Assert.Equal(period, actual.Period);
        Assert.Equal(description, actual.Description);
    }

    [Fact]
    public async Task Update_ShouldClearDescription_WhenNull()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);
        var current = record.Periods[0];
        var setBody = new RecordPeriodUpdateRequest
        {
            Description = new UpdateField<RecordPeriodDescription?>(RecordPeriodDescription.Parse("temp", null))
        };
        (await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], current.Id, setBody)).ShouldSuccess();

        var clearBody = new RecordPeriodUpdateRequest
        {
            Description = new UpdateField<RecordPeriodDescription?>(null)
        };
        var response = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], current.Id, clearBody);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.Null((await GetRecordAsync(AddedRecordIds[0])).Periods[0].Description);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenPeriodOverlaps()
    {
        var added = (await Context.Client.Records.AddPeriodAsync(
            AddedRecordIds[0], new RecordPeriodItem(CreateAfterAddedPeriods(2, 1)))).Unwrap();
        var first = (await GetRecordAsync(AddedRecordIds[0])).Periods[0].Period;
        var body = new RecordPeriodUpdateRequest
        {
            Period = new UpdateField<DateTimePeriod>(new DateTimePeriod(first.EndAt.AddMinutes(-1), first.EndAt.AddHours(4)))
        };

        var response = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], added.Id, body);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenPeriodHasZeroDuration()
    {
        var current = (await GetRecordAsync(AddedRecordIds[0])).Periods[0];
        var body = new RecordPeriodUpdateRequest
        {
            Period = new UpdateField<DateTimePeriod>(new DateTimePeriod(current.Period.StartAt, current.Period.StartAt))
        };

        var response = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], current.Id, body);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Add_ShouldFail_WhenPeriodHasZeroDuration()
    {
        var startAt = CreateAfterAddedPeriods(1, 1).StartAt;
        var body = new RecordPeriodItem(new DateTimePeriod(startAt, startAt));

        var response = await Context.Client.Records.AddPeriodAsync(AddedRecordIds[0], body);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Operations_ShouldFail_WhenRecordBelongsToAnotherUser()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);
        var periodId = record.Periods[0].Id;
        var body = new RecordPeriodItem(CreateAfterAddedPeriods(1, 1));
        var updateBody = new RecordPeriodUpdateRequest
        {
            Description = new UpdateField<RecordPeriodDescription?>(RecordPeriodDescription.Parse("foreign", null))
        };

        await Context.Preparer.Users.RegisterAsync();

        var add = await Context.Client.Records.AddPeriodAsync(AddedRecordIds[0], body);
        var update = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], periodId, updateBody);
        var remove = await Context.Client.Records.RemovePeriodAsync(AddedRecordIds[0], periodId);

        add.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        update.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        remove.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenNothingToDo()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);

        var response = await Context.Client.Records.UpdatePeriodAsync(
            AddedRecordIds[0], record.Periods[0].Id, new RecordPeriodUpdateRequest());

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenPeriodNotExists()
    {
        var body = new RecordPeriodUpdateRequest
        {
            Period = new UpdateField<DateTimePeriod>(CreateAfterAddedPeriods(1, 1))
        };

        var response = await Context.Client.Records.UpdatePeriodAsync(AddedRecordIds[0], RecordPeriodId.Generate(), body);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Remove_ShouldSuccess()
    {
        var added = (await Context.Client.Records.AddPeriodAsync(
            AddedRecordIds[0], new RecordPeriodItem(CreateAfterAddedPeriods(1, 1)))).Unwrap();

        var response = await Context.Client.Records.RemovePeriodAsync(AddedRecordIds[0], added.Id);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.Single((await GetRecordAsync(AddedRecordIds[0])).Periods);
    }

    [Fact]
    public async Task Remove_ShouldFail_WhenLastPeriod()
    {
        var record = await GetRecordAsync(AddedRecordIds[0]);

        var response = await Context.Client.Records.RemovePeriodAsync(AddedRecordIds[0], record.Periods[0].Id);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Remove_ShouldFail_WhenPeriodNotExists()
    {
        var response = await Context.Client.Records.RemovePeriodAsync(AddedRecordIds[0], RecordPeriodId.Generate());

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
