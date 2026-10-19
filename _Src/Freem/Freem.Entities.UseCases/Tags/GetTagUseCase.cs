using Freem.Domain.Storage.Models.Identifiers;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.Get;

namespace Freem.Entities.UseCases.Tags;

internal sealed class GetTagUseCase 
    : IEntitiesUseCase<GetTagRequest, GetTagResponse, GetTagErrorCode>
{
    private readonly ITagRepository _repository;

    public GetTagUseCase(ITagRepository repository)
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