using Freem.Domain.Storage.Repositories.Events;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Events.List;

namespace Freem.Entities.UseCases.Events;

internal sealed class ListEventUseCase 
    : IEntitiesUseCase<ListEventRequest, ListEventResponse, ListEventErrorCode>
{
    private readonly IEventRepository _repository;

    public ListEventUseCase(
        IEventRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        
        _repository = repository;
    }

    public async Task<ListEventResponse> ExecuteAsync(
        UseCaseExecutionContext context, ListEventRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var filter = new EventAfterTimeFilter(context.UserId, request.After)
        {
            Limit = (int)request.Limit
        };

        var result = await _repository.FindAsync(filter, cancellationToken);
        var events = await result.ToArrayAsync(cancellationToken);
        return ListEventResponse.CreateSuccess(events, result.TotalCount);
    }
}