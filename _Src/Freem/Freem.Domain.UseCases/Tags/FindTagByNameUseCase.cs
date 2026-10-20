using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.GetByName;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Tags;

internal sealed class FindTagByNameUseCase
    : IRequestHandler<HandlerContext, FindTagByNameRequest, Result<FindTagByNameResponse, Error>>
{
    private readonly ITagRepository _repository;

    public FindTagByNameUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<FindTagByNameResponse, Error>> HandleAsync(
        HandlerContext context, FindTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new TagFilter(context.UserId, request.SearchText);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tags = await result.Value.ToArrayAsync(cancellationToken);
        return new FindTagByNameResponse(tags, result.Value.Count);
    }
}