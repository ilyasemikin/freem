using System.Linq.Expressions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Extensions;

internal static class DatabaseContextExtensions
{
    public static async Task<TEvent?> FindEventAsync<TEvent>(
        this IQueryable<BaseEventEntity> queryable, 
        Expression<Func<TEvent, bool>> predicate)
        where TEvent : BaseEventEntity
    {
        return await queryable
            .Where(e => e is TEvent)
            .Cast<TEvent>()
            .FirstOrDefaultAsync(predicate);
    }
}