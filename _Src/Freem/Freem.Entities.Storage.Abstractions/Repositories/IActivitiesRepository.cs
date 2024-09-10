using Freem.Entities.Identifiers;
using Freem.Entities.Identifiers.Multiple;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IActivitiesRepository :
    IWriteRepository<Activity, ActivityIdentifier>,
    ISearchByIdRepository<Activity, ActivityIdentifier>
{
    Task<SearchEntityResult<Activity>> FindAsync(
        ActivityAndUserIdentifiers ids,
        CancellationToken cancellationToken);

    Task<SearchEntitiesAsyncResult<Activity>> FindByUserAsync(
        ActivitiesByUserFilter filter, 
        CancellationToken cancellationToken = default);
}
