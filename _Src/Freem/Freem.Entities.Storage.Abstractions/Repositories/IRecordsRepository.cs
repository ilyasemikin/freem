using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRecordsRepository :
    IWriteRepository<Record, RecordIdentifier>,
    ISearchByIdRepository<Record, RecordIdentifier>
{
    Task<SearchEntityResult<Record>> FindAsync(
        RecordAndUserIdentifiers ids,
        CancellationToken cancellationToken = default);
    
    Task<SearchEntitiesAsyncResult<Record>> FindByUserAsync(
        RecordsByUserFilter filter,
        CancellationToken cancellationToken = default);
}
