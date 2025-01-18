using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Tags;

internal sealed class GetTagUseCase 
    : IEntitiesUseCase<GetTagRequest, GetTagResponse, GetTagErrorCode>
{
    private readonly ISearchByMultipleIdsRepository<Tag, TagIdentifier, TagAndUserIdentifiers> _repository;

    public GetTagUseCase(ISearchByMultipleIdsRepository<Tag, TagIdentifier, TagAndUserIdentifiers> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new TagAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        return GetTagResponse.Create(result);
    }
}