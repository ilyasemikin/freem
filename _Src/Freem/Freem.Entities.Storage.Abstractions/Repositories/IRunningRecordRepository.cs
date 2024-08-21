using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRunningRecordRepository : IBaseWriteRepository<RunningRecord, UserIdentifier>
{
    Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(
        UserIdentifier userId,
        CancellationToken cancellationToken);
}
