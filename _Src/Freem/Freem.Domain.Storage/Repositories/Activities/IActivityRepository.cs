using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Ids;

namespace Freem.Domain.Storage.Repositories.Activities;

public interface IActivityRepository
{
    Task CreateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ActivityId id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Activity>> FindByIdAsync(
        ActivityId id,
        CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Activity>> FindByMultipleIdAsync(
        ActivityAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Activity>> FindAsync(
        ActivityByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Activity>> FindAsync(
        ActivityFilter filter,
        CancellationToken cancellationToken = default);
}