using System.Linq.Expressions;
using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Users;
using Freem.Domain.Records;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Extensions;

internal static class SearchEntitiesDatabaseContextExtensions
{
    public static async Task<ActivityEntity?> FindEntityAsync(
        this IQueryable<ActivityEntity> queryable, 
        Expression<Func<ActivityEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public static async Task<ActivityEntity?> FindEntityAsync(
        this IQueryable<ActivityEntity> queryable, 
        Activity activity,
        CancellationToken cancellationToken = default)
    {
        var id = activity.Id.ToString();
        var userId = activity.UserId.ToString();
        return await queryable.FindEntityAsync(
            e => e.Id == id && e.UserId == userId, 
            cancellationToken);
    }

    public static async Task<ActivityEntity?> FindEntityAsync(
        this IQueryable<ActivityEntity> queryable,
        ActivityId id,
        CancellationToken cancellationToken = default)
    {
        var value = id.ToString();
        return await queryable.FindEntityAsync(e => e.Id == value, cancellationToken);
    }

    public static async Task<RecordEntity?> FindEntityAsync(
        this IQueryable<RecordEntity> queryable,
        Expression<Func<RecordEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public static async Task<RecordEntity?> FindEntityAsync(
        this IQueryable<RecordEntity> queryable,
        Record record,
        CancellationToken cancellationToken = default)
    {
        var id = record.Id.ToString();
        var userId = record.UserId.ToString();
        return await queryable.FindEntityAsync(
            e => e.Id == id && e.UserId == userId,
            cancellationToken);
    }

    public static async Task<RecordEntity?> FindEntityAsync(
        this IQueryable<RecordEntity> queryable,
        RecordId id,
        CancellationToken cancellationToken = default)
    {
        var value = id.ToString();
        return await queryable.FindEntityAsync(e => e.Id == value, cancellationToken);
    }

    public static async Task<RunningRecordEntity?> FindEntityAsync(
        this IQueryable<RunningRecordEntity> queryable,
        Expression<Func<RunningRecordEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public static async Task<RunningRecordEntity?> FindEntityAsync(
        this IQueryable<RunningRecordEntity> queryable,
        RunningRecord record,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FindEntityAsync(record.UserId, cancellationToken);
    }
    
    public static async Task<RunningRecordEntity?> FindEntityAsync(
        this IQueryable<RunningRecordEntity> queryable,
        UserId id,
        CancellationToken cancellationToken = default)
    {
        var value = id.ToString();
        return await queryable.FindEntityAsync(e => e.UserId == value, cancellationToken);
    }

    public static async Task<TagEntity?> FindEntityAsync(
        this IQueryable<TagEntity> queryable,
        Expression<Func<TagEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public static async Task<TagEntity?> FindEntityAsync(
        this IQueryable<TagEntity> queryable,
        Tag tag,
        CancellationToken cancellationToken = default)
    {
        var id = tag.Id.ToString();
        var userId = tag.UserId.ToString();
        return await queryable.FindEntityAsync(
            e => e.Id == id && e.UserId == userId,
            cancellationToken);
    }
    
    public static async Task<TagEntity?> FindEntityAsync(
        this IQueryable<TagEntity> queryable,
        TagId id,
        CancellationToken cancellationToken = default)
    {
        var value = id.ToString();
        return await queryable.FindEntityAsync(e => e.Id == value, cancellationToken);
    }
    
    public static async Task<UserEntity?> FindEntityAsync(
        this IQueryable<UserEntity> queryable,
        Expression<Func<UserEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public static async Task<UserEntity?> FindEntityAsync(
        this IQueryable<UserEntity> queryable,
        User user,
        CancellationToken cancellationToken = default)
    {
        var userId = user.Id.ToString();
        return await queryable.FindEntityAsync(e => e.Id == userId, cancellationToken);
    }
}