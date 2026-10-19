using Freem.Domain.Storage.Repositories.Tags;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.List;

namespace Freem.Entities.UseCases.Tags;

internal sealed class ListTagUseCase 
    : IEntitiesUseCase<ListTagRequest, ListTagResponse, ListTagErrorCode>
{
    private readonly ITagRepository _repository;

    public ListTagUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new TagByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset
        };
            
        var result = await _repository.FindAsync(filter, cancellationToken);
        var tags = await result.ToArrayAsync(cancellationToken);
        return ListTagResponse.CreateSuccess(tags, result.TotalCount);
    }
}