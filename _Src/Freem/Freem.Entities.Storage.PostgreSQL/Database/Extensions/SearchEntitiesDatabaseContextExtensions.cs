using System.Linq.Expressions;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
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
        ActivityIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        var id = identifier.ToString();
        return await queryable.FindEntityAsync(e => e.Id == id, cancellationToken);
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
        RecordIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        var id = identifier.ToString();
        return await queryable.FindEntityAsync(e => e.Id == id, cancellationToken);
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
        UserIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        var id = identifier.ToString();
        return await queryable.FindEntityAsync(e => e.UserId == id, cancellationToken);
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
        TagIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        var id = identifier.ToString();
        return await queryable.FindEntityAsync(e => e.Id == id, cancellationToken);
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