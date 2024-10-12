using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Activities.List.Models;
using Freem.Entities.UseCases.Context;

namespace Freem.Entities.UseCases.Activities.List;

internal sealed class ListActivityUseCase : IUseCase<ListActivityRequest, ListActivityResponse>
{
    private readonly IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesByUserFilter> _repository;

    public ListActivityUseCase(
        IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesByUserFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = Map(context, request);
        var result = await _repository.FindAsync(filter, cancellationToken);
        return new ListActivityResponse(result, result.TotalCount);
    }

    private static ActivitiesByUserFilter Map(UseCaseExecutionContext context, ListActivityRequest request)
    {
        return new ActivitiesByUserFilter(context.UserId)
        {
            Limit = request.Limit,
            Offset = request.Offset
        };
    }
}