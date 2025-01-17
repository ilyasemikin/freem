using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DTO.Events.List;
using Freem.Entities.Users.Identifiers;
using Freem.Linq;

namespace Freem.Entities.UseCases.Events;

internal sealed class ListEventUseCase : IUseCase<ListEventRequest, ListEventResponse, ListEventErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier, EventsAfterTimeFilter> _repository;

    public ListEventUseCase(
        IMultipleSearchByFilterRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier, EventsAfterTimeFilter> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListEventResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListEventRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new EventsAfterTimeFilter(context.UserId, request.After)
        {
            Limit = (int)request.Limit
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        var events = await result.ToArrayAsync(cancellationToken);
        return ListEventResponse.CreateSuccess(events, result.TotalCount);
    }
}