using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Events.List.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.List;

internal sealed class ListEventUseCase : IUseCase<ListEventRequest, ListEventResponse>
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
        
        var filter = Map();
        var result = await _repository.FindAsync(filter, cancellationToken);
        return new ListEventResponse(result, result.TotalCount);
        
        EventsAfterTimeFilter Map()
        {
            return new EventsAfterTimeFilter(context.UserId, request.After)
            {
                Limit = request.Limit
            };
        }
    }
}