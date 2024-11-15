using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Tags.Get.Models;

namespace Freem.Entities.UseCases.Tags.Get;

internal sealed class GetTagUseCase : IUseCase<GetTagRequest, GetTagResponse>
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
        return new GetTagResponse(result.Entity);
    }
}