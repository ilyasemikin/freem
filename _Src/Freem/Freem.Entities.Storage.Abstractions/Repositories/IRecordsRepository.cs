using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRecordsRepository :
    IWriteRepository<Record, RecordIdentifier>,
    ISearchByIdRepository<Record, RecordIdentifier>,
    ISearchByMultipleIdsRepository<Record, RecordIdentifier, RecordAndUserIdentifiers>,
    IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByUserFilter>,
    IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter>
{
}
