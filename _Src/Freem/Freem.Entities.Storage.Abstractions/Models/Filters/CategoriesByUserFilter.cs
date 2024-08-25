using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class CategoriesByUserFilter : ILimitFilter, IOffsetFilter, ISortingFilter<CategorySortField>
{
    public static readonly SortOptions<CategorySortField> DefaultSorting = new(
        CategorySortField.CreatedAt,
        SortOrder.Descending);

    private readonly SortOptions<CategorySortField> _sorting = DefaultSorting;
    
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
    
    public SortOptions<CategorySortField> Sorting
    {
        get => _sorting;
        init
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sorting = value;
        }
    }
    
    public UserIdentifier UserId { get; }

    public CategoriesByUserFilter(UserIdentifier userId) 
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}