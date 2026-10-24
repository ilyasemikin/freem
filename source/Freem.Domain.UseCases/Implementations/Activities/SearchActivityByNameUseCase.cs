using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Search;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Activities;

internal sealed class SearchActivityByNameUseCase 
    : IRequestHandler<HandlerContext, SearchActivityByNameRequest, Result<SearchActivityByNameResponse, Error>>
{
    private readonly IActivityRepository _repository;

    public SearchActivityByNameUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<SearchActivityByNameResponse, Error>> HandleAsync(
        HandlerContext context, SearchActivityByNameRequest request,
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
        return new SearchActivityByNameResponse(activities, result.Value.Count);
    }
}