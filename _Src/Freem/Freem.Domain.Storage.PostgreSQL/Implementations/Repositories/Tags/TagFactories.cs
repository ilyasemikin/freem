using System.Linq.Expressions;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Tags;

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