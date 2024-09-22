using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRunningRecordRepository : IWriteRepository<RunningRecord, UserIdentifier>
{
    Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(
        UserIdentifier userId,
        CancellationToken cancellationToken = default);
}
