using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class FindActivityUseCase 
    : IRequestHandler<HandlerContext, FindActivityRequest, Result<FindActivityResponse, Error>>
{
    private readonly IActivityRepository _repository;

    public FindActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<FindActivityResponse, Error>> HandleAsync(
        HandlerContext context, FindActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new ActivityFilter(context.UserId, request.SearchText)
        {
            Limit = Limit.Default
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var activities = await result.Value.ToArrayAsync(cancellationToken);
        return new FindActivityResponse(activities, result.Value.Count);
    }
}