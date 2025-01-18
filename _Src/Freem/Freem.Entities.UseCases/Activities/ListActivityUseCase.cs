using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Linq;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class ListActivityUseCase 
    : IEntitiesUseCase<ListActivityRequest, ListActivityResponse, ListActivityErrorCode>
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
        context.ThrowsIfUnauthorized();

        var filter = new ActivitiesByUserFilter(context.UserId)
        {
            Offset = (int)request.Offset,
            Limit = (int)request.Limit
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        var activities = await result.ToArrayAsync(cancellationToken);
        return ListActivityResponse.CreateSuccess(activities, result.TotalCount);
    }
}