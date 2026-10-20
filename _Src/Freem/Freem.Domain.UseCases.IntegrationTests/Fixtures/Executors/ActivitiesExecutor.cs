using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Activities;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Archive;
using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Domain.UseCases.Contracts.Activities.Unarchive;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class ActivitiesExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public ActivitiesExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<Activity> CreateAsync(HandlerContext context, CreateActivityRequest request)
    {
        var result = await _mediator.ExecuteAsync<CreateActivityRequest, Result<CreateActivityResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Activity;
    }

    public async Task<Activity> CreateAsync(HandlerContext context)
    {
        var value = Guid.NewGuid().ToString();
        var name = ActivityName.Parse(value, null);

        var request = new CreateActivityRequest(name);
        return await CreateAsync(context, request);
    }

    public async Task<IReadOnlyList<Activity>> CreateManyAsync(HandlerContext context, int count)
    {
        var activities = new List<Activity>();
        foreach (var _ in Enumerable.Range(0, count))
        {
            var activity = await CreateAsync(context);
            activities.Add(activity);
        }

        return activities;
    }

    public async Task ArchiveAsync(HandlerContext context, ActivityId id)
    {
        var request = new ArchiveActivityRequest(id);
        var result = await _mediator.ExecuteAsync<ArchiveActivityRequest, Result<Error>>(context, request);
        result.EnsureSuccess();
    }

    public async Task UnarchiveAsync(HandlerContext context, ActivityId id)
    {
        var request = new UnarchiveActivityRequest(id);
        var result = await _mediator.ExecuteAsync<UnarchiveActivityRequest, Result<Error>>(context, request);
        result.EnsureSuccess();
    }

    public async Task<Activity> RequiredGetAsync(HandlerContext context, ActivityId id)
    {
        var request = new GetActivityRequest(id);
        var result = await _mediator.ExecuteAsync<GetActivityRequest, Result<GetActivityResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Activity;
    }
}