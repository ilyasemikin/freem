using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Tags;

internal sealed class ListTagUseCase 
    : IRequestHandler<HandlerContext, ListTagRequest, Result<ListTagResponse, Error>>
{
    private readonly ITagRepository _repository;

    public ListTagUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<ListTagResponse, Error>> HandleAsync(
        HandlerContext context, ListTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new TagByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tags = await result.Value.ToArrayAsync(cancellationToken);
        return new ListTagResponse(tags, result.Value.Count);
    }
}