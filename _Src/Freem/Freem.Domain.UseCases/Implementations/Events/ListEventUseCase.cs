using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.List;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Events;

internal sealed class ListEventUseCase
    : IRequestHandler<HandlerContext, ListEventRequest, Result<ListEventResponse, Error>>
{
    private readonly IEventRepository _repository;

    public ListEventUseCase(IEventRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<Result<ListEventResponse, Error>> HandleAsync(
        HandlerContext context, ListEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new EventByUserFilter(context.UserId)
        {
            Limit = (int)request.Limit,
            Offset = (int)request.Offset,
            EntityType = request.EntityType,
            EntityRef = request.EntityRef,
            From = request.From,
            To = request.To
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        if (!result.Success)
            return result.Error;

        var events = await result.Value.ToArrayAsync(cancellationToken);
        return new ListEventResponse(events, result.Value.Count);
    }
}
