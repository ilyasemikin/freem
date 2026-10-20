using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.RunningRecords;

public interface IRunningRecordRepository
{
    Task<Result<Error>> CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(RunningRecordId id, CancellationToken cancellationToken = default);

    Task<Result<RunningRecord, Error>> FindByIdAsync(
        RunningRecordId id,
        CancellationToken cancellationToken = default);
}