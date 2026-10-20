using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Events;

internal sealed class GetEventUseCase
    : IRequestHandler<HandlerContext, GetEventRequest, Result<GetEventResponse, Error>>
{
    private readonly IEventRepository _repository;

    public GetEventUseCase(IEventRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<GetEventResponse, Error>> HandleAsync(
        HandlerContext context, GetEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindByIdAsync(request.Id, cancellationToken);
        if (!result.Success)
            return result.Error;

        var @event = result.Value;
        if (@event.UserId != context.UserId)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(request.Id);

        return new GetEventResponse(@event);
    }
}
