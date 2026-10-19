using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Identifiers;

namespace Freem.Domain.Storage.Repositories.Activities;

public interface IActivityRepository
{
    Task CreateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ActivityIdentifier id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Activity>> FindByIdAsync(
        ActivityIdentifier id,
        CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Activity>> FindByMultipleIdAsync(
        ActivityAndUserIdentifiers ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Activity>> FindAsync(
        ActivityByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Activity>> FindAsync(
        ActivityFilter filter,
        CancellationToken cancellationToken = default);
}