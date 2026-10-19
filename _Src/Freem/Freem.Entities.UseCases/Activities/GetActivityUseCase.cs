using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Get;

namespace Freem.Entities.UseCases.Activities;

internal sealed class GetActivityUseCase 
    : IEntitiesUseCase<GetActivityRequest, GetActivityResponse, GetActivityErrorCode>
{
    private readonly IActivityRepository _repository;

    public GetActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<GetActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, GetActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new ActivityAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        return GetActivityResponse.Create(result);
    }
}