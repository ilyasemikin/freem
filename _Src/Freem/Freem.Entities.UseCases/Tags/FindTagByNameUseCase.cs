using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.Linq;

namespace Freem.Entities.UseCases.Tags;

internal sealed class FindTagByNameUseCase
    : IEntitiesUseCase<FindTagByNameRequest, FindTagByNameResponse, FindTagByNameErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsFilter> _repository;

    public FindTagByNameUseCase(ITagsRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<FindTagByNameResponse> ExecuteAsync(
        UseCaseExecutionContext context, FindTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var filter = new TagsFilter(context.UserId, request.SearchText);
        
        var result = await _repository.FindAsync(filter, cancellationToken);
        var tags = await result.ToArrayAsync(cancellationToken);
        return FindTagByNameResponse.CreateSuccess(tags, result.TotalCount);
    }
}