using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.Get;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class RecordsExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public RecordsExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<Domain.Entities.Records.Record> CreateAsync(HandlerContext context, CreateRecordRequest request)
    {
        var result = await _mediator.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Record;
    }

    public async Task<Domain.Entities.Records.Record> CreateAsync(HandlerContext context, IEnumerable<ActivityId> activityIds)
    {
        var now = DateTimeOffset.UtcNow;
        var startAt = now.AddDays(-1);

        var period = new DateTimePeriod(startAt, now);
        var activities = new RelatedActivityIdsCollection(activityIds);

        var request = new CreateRecordRequest(period, activities);
        return await CreateAsync(context, request);
    }

    public async Task<IReadOnlyList<Domain.Entities.Records.Record>> CreateManyAsync(
        HandlerContext context, IReadOnlyList<ActivityId> activityIds, int count)
    {
        var records = new List<Domain.Entities.Records.Record>();
        foreach (var _ in Enumerable.Range(0, count))
        {
            var record = await CreateAsync(context, activityIds);
            records.Add(record);
        }

        return records;
    }

    public async Task<Domain.Entities.Records.Record> RequiredGetAsync(HandlerContext context, RecordId id)
    {
        var request = new GetRecordRequest(id);
        var result = await _mediator.ExecuteAsync<GetRecordRequest, Result<GetRecordResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Record;
    }
}