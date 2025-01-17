using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DTO.Tags.List;
using Freem.Linq;

namespace Freem.Entities.UseCases.Tags;

internal sealed class ListTagUseCase : IUseCase<ListTagRequest, ListTagResponse, ListTagErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsByUserFilter> _repository;

    public ListTagUseCase(IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsByUserFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new TagsByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        var tags = await result.ToArrayAsync(cancellationToken);
        return ListTagResponse.CreateSuccess(tags, result.TotalCount);
    }
}