using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class ListActivityUseCase 
    : IRequestHandler<HandlerContext, ListActivityRequest, Result<ListActivityResponse, Error>>
{
    private readonly IActivityRepository _repository;

    public ListActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<ListActivityResponse, Error>> HandleAsync(
        HandlerContext context, ListActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new ActivityByUserFilter(context.UserId)
        {
            Offset = (int)request.Offset,
            Limit = (int)request.Limit
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var activities = await result.Value.ToArrayAsync(cancellationToken);
        return new ListActivityResponse(activities, result.Value.Count);
    }
}