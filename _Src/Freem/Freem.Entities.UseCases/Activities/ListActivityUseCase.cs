using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.List;

namespace Freem.Entities.UseCases.Activities;

internal sealed class ListActivityUseCase 
    : IEntitiesUseCase<ListActivityRequest, ListActivityResponse, ListActivityErrorCode>
{
    private readonly IActivityRepository _repository;

    public ListActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new ActivityByUserFilter(context.UserId)
        {
            Offset = (int)request.Offset,
            Limit = (int)request.Limit
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        var activities = await result.ToArrayAsync(cancellationToken);
        return ListActivityResponse.CreateSuccess(activities, result.TotalCount);
    }
}