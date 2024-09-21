using System.Linq.Expressions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
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
        return await queryable.FindEntityAsync(
            e => e.Id == activity.Id.Value && e.UserId == activity.UserId.Value, 
            cancellationToken);
    }

    public static async Task<ActivityEntity?> FindEntityAsync(
        this IQueryable<ActivityEntity> queryable,
        ActivityIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FindEntityAsync(e => e.Id == identifier.Value, cancellationToken);
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
        return await queryable.FindEntityAsync(
            e => e.Id == record.Id.Value && e.UserId == record.UserId.Value,
            cancellationToken);
    }

    public static async Task<RecordEntity?> FindEntityAsync(
        this IQueryable<RecordEntity> queryable,
        RecordIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FindEntityAsync(e => e.Id == identifier.Value, cancellationToken);
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
        return await queryable.FindEntityAsync(e => e.UserId == identifier.Value, cancellationToken);
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
        return await queryable.FindEntityAsync(
            e => e.Id == tag.Id.Value && e.UserId == tag.UserId.Value,
            cancellationToken);
    }
    
    public static async Task<TagEntity?> FindEntityAsync(
        this IQueryable<TagEntity> queryable,
        TagIdentifier identifier,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FindEntityAsync(e => e.Id == identifier.Value, cancellationToken);
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
        return await queryable.FindEntityAsync(e => e.Id == user.Id.Value, cancellationToken);
    }
    
    public static async Task<TEvent?> FindEntityAsync<TEvent>(
        this IQueryable<BaseEventEntity> queryable, 
        Expression<Func<TEvent, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TEvent : BaseEventEntity
    {
        return await queryable
            .Where(e => e is TEvent)
            .Cast<TEvent>()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }
}