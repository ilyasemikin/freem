using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Models;

namespace Freem.Domain.Storage.Repositories.RunningRecords;

public interface IRunningRecordRepository
{
    Task CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(RunningRecordId id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<RunningRecord>> FindByIdAsync(
        RunningRecordId id,
        CancellationToken cancellationToken = default);
}