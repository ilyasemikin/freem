using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IActivitiesRepository :
    IWriteRepository<Activity, ActivityIdentifier>,
    ISearchByIdRepository<Activity, ActivityIdentifier>,
    ISearchByMultipleIdsRepository<Activity, ActivityIdentifier, ActivityAndUserIdentifiers>,
    IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesByUserFilter>
{
}
