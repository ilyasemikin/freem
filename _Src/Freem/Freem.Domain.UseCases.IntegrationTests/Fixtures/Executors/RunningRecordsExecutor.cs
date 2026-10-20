using Freem.Domain.Activities;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Get;
using Freem.Domain.UseCases.Contracts.RunningRecords.Remove;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class RunningRecordsExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public RunningRecordsExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<RunningRecord> StartAsync(HandlerContext context, StartRunningRecordRequest request)
    {
        var result = await _mediator.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.RunningRecord;
    }

    public async Task<RunningRecord> StartAsync(HandlerContext context, IEnumerable<ActivityId> activityIds)
    {
        var startAt = DateTimeOffset.UtcNow.AddDays(-1);
        var activities = new RelatedActivityIdsCollection(activityIds);

        var request = new StartRunningRecordRequest(startAt, activities);
        return await StartAsync(context, request);
    }

    public async Task RemoveAsync(HandlerContext context)
    {
        var request = RemoveRunningRecordRequest.Instance;
        var result = await _mediator.ExecuteAsync<RemoveRunningRecordRequest, Result<Error>>(context, request);
        result.EnsureSuccess();
    }

    public async Task<Result<GetRunningRecordResponse, Error>> GetAsync(HandlerContext context)
    {
        var request = GetRunningRecordRequest.Instance;
        return await _mediator.ExecuteAsync<GetRunningRecordRequest, Result<GetRunningRecordResponse, Error>>(context, request);
    }

    public async Task<RunningRecord> RequiredGetAsync(HandlerContext context)
    {
        var result = await GetAsync(context);
        var response = result.EnsureSuccess();
        return response.Record;
    }
}