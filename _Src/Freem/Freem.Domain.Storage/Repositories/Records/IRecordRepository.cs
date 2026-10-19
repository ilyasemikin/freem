using Freem.Domain.Entities.Records;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Identifiers;

namespace Freem.Domain.Storage.Repositories.Records;

public interface IRecordRepository
{
    Task CreateAsync(Record entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Record entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(RecordIdentifier id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Record>> FindByIdAsync(RecordIdentifier id, CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Record>> FindByMultipleIdAsync(
        RecordAndUserIdentifiers ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Record>> FindAsync(
        RecordByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Record>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default);
}