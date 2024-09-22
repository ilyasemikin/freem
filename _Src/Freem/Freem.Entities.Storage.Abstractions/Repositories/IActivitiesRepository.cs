using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IActivitiesRepository :
    IWriteRepository<Activity, ActivityIdentifier>,
    ISearchByIdRepository<Activity, ActivityIdentifier>
{
    Task<SearchEntityResult<Activity>> FindAsync(
        ActivityAndUserIdentifiers ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Activity>> FindByUserAsync(
        ActivitiesByUserFilter filter, 
        CancellationToken cancellationToken = default);
}
