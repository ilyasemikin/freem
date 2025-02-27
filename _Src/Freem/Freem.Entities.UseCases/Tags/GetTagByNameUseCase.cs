using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;

namespace Freem.Entities.UseCases.Tags;

internal sealed class GetTagByNameUseCase
    : IEntitiesUseCase<GetTagByNameRequest, GetTagByNameResponse, GetTagByNameErrorCode>
{
    private readonly ITagsRepository _repository;

    public GetTagByNameUseCase(ITagsRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetTagByNameResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var result = await _repository.FindByNameAsync(request.Name, cancellationToken);
        return GetTagByNameResponse.Create(result);
    }
}