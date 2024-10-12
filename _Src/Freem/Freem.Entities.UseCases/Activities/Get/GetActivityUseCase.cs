using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Activities.Get.Models;
using Freem.Entities.UseCases.Context;

namespace Freem.Entities.UseCases.Activities.Get;

internal sealed class GetActivityUseCase : IUseCase<GetActivityRequest, GetActivityResponse>
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
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        return new GetActivityResponse(result.Entity);
    }
}