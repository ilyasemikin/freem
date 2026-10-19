using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Find;

namespace Freem.Entities.UseCases.Activities;

internal sealed class FindActivityUseCase 
    : IEntitiesUseCase<FindActivityRequest, FindActivityResponse, FindActivityErrorCode>
{
    private readonly IActivityRepository _repository;

    public FindActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<FindActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, FindActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new ActivityFilter(context.UserId, request.SearchText)
        {
            Limit = Limit.Default
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        var activities = await result.ToArrayAsync(cancellationToken);
        return FindActivityResponse.CreateSuccess(activities, result.TotalCount);
    }
}