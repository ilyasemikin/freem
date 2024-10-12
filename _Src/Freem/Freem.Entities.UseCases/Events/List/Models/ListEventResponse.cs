using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.UseCases.Models.Filter;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.List.Models;

public sealed class ListEventResponse : IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>
{
    private readonly IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>> _entities;

    public TotalCount TotalCount { get; }
    
    public ListEventResponse(
        IAsyncEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>> entities, 
        TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(totalCount);

        _entities = entities;
        
        TotalCount = totalCount;
    }
    
    public IAsyncEnumerator<IEntityEvent<IEntityIdentifier, UserIdentifier>> GetAsyncEnumerator(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}