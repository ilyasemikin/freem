using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRunningRecordRepository : IWriteRepository<RunningRecord, UserIdentifier>
{
    Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(
        UserIdentifier userId,
        CancellationToken cancellationToken = default);
}
