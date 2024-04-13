using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions;

public interface IRunningRecordRepository : IBaseWriteRepository<RunningRecord>
{
    Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(string id, CancellationToken cancellationToken);
}
