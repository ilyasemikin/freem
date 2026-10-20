using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models.Ids;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Activities;

public interface IActivityRepository
{
    Task<Result<Error>> CreateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> UpdateAsync(Activity entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(ActivityId id, CancellationToken cancellationToken = default);

    Task<Result<Activity, Error>> FindByIdAsync(
        ActivityId id,
        CancellationToken cancellationToken = default);
    Task<Result<Activity, Error>> FindByMultipleIdAsync(
        ActivityAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<Activity>, Error>> FindAsync(
        ActivityByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<Activity>, Error>> FindAsync(
        ActivityFilter filter,
        CancellationToken cancellationToken = default);
}