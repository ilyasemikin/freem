using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Freem.Linq;

namespace Freem.Entities.UseCases.Activities;

internal sealed class FindActivityUseCase 
    : IEntitiesUseCase<FindActivityRequest, FindActivityResponse, FindActivityErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesFilter> _repository;

    public FindActivityUseCase(
        IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<FindActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, FindActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new ActivitiesFilter(context.UserId, request.SearchText)
        {
            Limit = Limit.Default
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        var activities = await result.ToArrayAsync(cancellationToken);
        return FindActivityResponse.CreateSuccess(activities, result.TotalCount);
    }
}