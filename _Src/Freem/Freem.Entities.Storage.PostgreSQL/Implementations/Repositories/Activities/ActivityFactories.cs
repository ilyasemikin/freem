using System.Linq.Expressions;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityFactories
{
    public static Expression<Func<ActivityEntity, object?>> CreateSortSelector(ActivitySortField field)
    {
        return field switch
        {
            ActivitySortField.Id => e => e.Id,
            ActivitySortField.UserId => e => e.UserId,
            ActivitySortField.Name => e => e.Name,
            ActivitySortField.Status => e => e.Status,
            ActivitySortField.CreatedAt => e => e.CreatedAt,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}