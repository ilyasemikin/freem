using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRunningRecordRepository : IWriteRepository<RunningRecord, RunningRecordIdentifier>
{
    Task<SearchEntityResult<RunningRecord>> FindByIdAsync(
        RunningRecordIdentifier id,
        CancellationToken cancellationToken = default);
}
