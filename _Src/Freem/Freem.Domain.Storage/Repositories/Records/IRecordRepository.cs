using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Records;

public interface IRecordRepository
{
    Task<Result<Error>> CreateAsync(Record entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> CreateManyAsync(IReadOnlyList<Record> entities, CancellationToken cancellationToken = default);
    Task<Result<Error>> UpdateAsync(Record entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(RecordId id, CancellationToken cancellationToken = default);

    Task<Result<Record, Error>> FindByIdAsync(RecordId id, CancellationToken cancellationToken = default);
    Task<Result<Record, Error>> FindByMultipleIdAsync(
        RecordAndUserIds ids,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<Record>, Error>> FindAsync(
        RecordByUserFilter filter,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<Record>, Error>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default);
}