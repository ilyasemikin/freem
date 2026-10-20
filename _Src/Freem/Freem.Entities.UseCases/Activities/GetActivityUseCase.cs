using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class GetActivityUseCase 
    : IRequestHandler<HandlerContext, GetActivityRequest, Result<GetActivityResponse, Error>>
{
    private readonly IActivityRepository _repository;

    public GetActivityUseCase(
        IActivityRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<Result<GetActivityResponse, Error>> HandleAsync(
        HandlerContext context, GetActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new ActivityAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new GetActivityResponse(result.Value);
    }
}