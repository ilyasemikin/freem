using Freem.Entities.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRunningRecordRepository : 
    IWriteRepository<RunningRecord, RunningRecordIdentifier>,
    ISearchByIdRepository<RunningRecord, RunningRecordIdentifier>
{
}
