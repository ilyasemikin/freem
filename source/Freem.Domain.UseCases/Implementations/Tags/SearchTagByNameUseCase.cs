using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Search;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Tags;

internal sealed class SearchTagByNameUseCase
    : IRequestHandler<HandlerContext, SearchTagByNameRequest, Result<SearchTagByNameResponse, Error>>
{
    private readonly ITagRepository _repository;

    public SearchTagByNameUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<SearchTagByNameResponse, Error>> HandleAsync(
        HandlerContext context, SearchTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new TagFilter(context.UserId, request.SearchText);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tags = await result.Value.ToArrayAsync(cancellationToken);
        return new SearchTagByNameResponse(tags, result.Value.Count);
    }
}