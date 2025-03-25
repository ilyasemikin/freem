using System.Linq.Expressions;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal static class TagFactories
{
    public static Expression<Func<TagEntity, object>> CreateSortSelector(TagSortField field)
    {
        return field switch
        {
            TagSortField.Id => e => e.Id,
            TagSortField.Name => e => e.Name,
            TagSortField.CreatedAt => e => e.CreatedAt,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}