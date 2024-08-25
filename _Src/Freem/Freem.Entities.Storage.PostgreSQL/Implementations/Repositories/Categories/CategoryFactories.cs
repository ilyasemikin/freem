using System.Linq.Expressions;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal static class CategoryFactories
{
    public static Expression<Func<CategoryEntity, object?>> CreateSortSelector(CategorySortField field)
    {
        return field switch
        {
            CategorySortField.Id => e => e.Id,
            CategorySortField.UserId => e => e.UserId,
            CategorySortField.Name => e => e.Name,
            CategorySortField.Status => e => e.Status,
            CategorySortField.CreatedAt => e => e.CreatedAt,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}