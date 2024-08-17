using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRecordsRepository :
    IBaseWriteRepository<Record, RecordIdentifier>,
    IBaseSearchByIdRepository<Record, RecordIdentifier>,
    IBaseMultipleDeletionByUserRepository<Record, RecordIdentifier>
{
}
