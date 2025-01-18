using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class GetActivityUseCase 
    : IEntitiesUseCase<GetActivityRequest, GetActivityResponse, GetActivityErrorCode>
{
    private readonly ISearchByMultipleIdsRepository<Activity, ActivityIdentifier, ActivityAndUserIdentifiers> _repository;

    public GetActivityUseCase(
        ISearchByMultipleIdsRepository<Activity, ActivityIdentifier, ActivityAndUserIdentifiers> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        return GetActivityResponse.Create(result);
    }
}