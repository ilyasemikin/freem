using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Ids;

namespace Freem.Domain.Storage.Repositories.Records;

public interface IRecordRepository
{
    Task CreateAsync(Record entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Record entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(RecordId id, CancellationToken cancellationToken = default);

    Task<SearchEntityResult<Record>> FindByIdAsync(RecordId id, CancellationToken cancellationToken = default);
    Task<SearchEntityResult<Record>> FindByMultipleIdAsync(
        RecordAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<Record>> FindAsync(
        RecordByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<SearchEntitiesAsyncResult<Record>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default);
}