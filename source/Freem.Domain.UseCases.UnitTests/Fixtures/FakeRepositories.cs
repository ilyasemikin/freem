using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using RecordEntity = Freem.Domain.Entities.Records.Record;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Tags;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.UnitTests.Fixtures;

internal static class FakeBatch
{
    public static Result<AsyncBatch<T>, Error> Slice<T>(IReadOnlyList<T> all, int offset, int limit)
    {
        var page = all.Skip(offset).Take(limit).ToArray();
        var batch = new AsyncBatch<T>(AsyncItems.From(page), all.Count);
        return Result<AsyncBatch<T>, Error>.CreateSuccess(batch);
    }
}

internal sealed class FakeTagRepository : ITagRepository
{
    private readonly IReadOnlyList<Tag> _tags;

    public FakeTagRepository(IReadOnlyList<Tag> tags)
    {
        _tags = tags;
    }

    public Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        var result = FakeBatch.Slice(_tags, filter.Offset.Value, filter.Limit.Value);
        return Task.FromResult(result);
    }

    public Task<Result<Error>> CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> UpdateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> DeleteAsync(TagId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Tag, Error>> FindByIdAsync(TagId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Tag, Error>> FindByMultipleIdAsync(
        TagAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagFilter filter,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}

internal sealed class FakeActivityRepository : IActivityRepository
{
    private readonly IReadOnlyList<Activity> _activities;

    public FakeActivityRepository(IReadOnlyList<Activity> activities)
    {
        _activities = activities;
    }

    public Task<Result<AsyncBatch<Activity>, Error>> FindAsync(
        ActivityByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        var result = FakeBatch.Slice(_activities, filter.Offset.Value, filter.Limit.Value);
        return Task.FromResult(result);
    }

    public Task<Result<Error>> CreateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> UpdateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> DeleteAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Activity, Error>> FindByIdAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Activity, Error>> FindByMultipleIdAsync(
        ActivityAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<AsyncBatch<Activity>, Error>> FindAsync(
        ActivityFilter filter,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}

internal sealed class FakeRecordRepository : IRecordRepository
{
    private readonly IReadOnlyList<RecordEntity> _records;
    private readonly Error? _error;

    public FakeRecordRepository(IReadOnlyList<RecordEntity> records, Error? error = null)
    {
        _records = records;
        _error = error;
    }

    public Task<Result<AsyncBatch<RecordEntity>, Error>> FindAsync(
        RecordByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        if (_error is not null)
        {
            Result<AsyncBatch<RecordEntity>, Error> failure = _error;
            return Task.FromResult(failure);
        }

        var result = FakeBatch.Slice(_records, filter.Offset.Value, filter.Limit.Value);
        return Task.FromResult(result);
    }

    public Task<Result<Error>> CreateAsync(RecordEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> CreateManyAsync(IReadOnlyList<RecordEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> UpdateAsync(RecordEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<Error>> DeleteAsync(RecordId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<RecordEntity, Error>> FindByIdAsync(RecordId id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<RecordEntity, Error>> FindByMultipleIdAsync(
        RecordAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<Result<AsyncBatch<RecordEntity>, Error>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}
