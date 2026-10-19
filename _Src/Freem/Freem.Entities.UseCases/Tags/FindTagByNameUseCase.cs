using Freem.Domain.Storage.Repositories.Tags;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;

namespace Freem.Entities.UseCases.Tags;

internal sealed class FindTagByNameUseCase
    : IEntitiesUseCase<FindTagByNameRequest, FindTagByNameResponse, FindTagByNameErrorCode>
{
    private readonly ITagRepository _repository;

    public FindTagByNameUseCase(ITagRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<FindTagByNameResponse> ExecuteAsync(
        UseCaseExecutionContext context, FindTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new TagFilter(context.UserId, request.SearchText);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var tags = await result.ToArrayAsync(cancellationToken);
        return FindTagByNameResponse.CreateSuccess(tags, result.TotalCount);
    }
}