using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class ActivitiesFilter : ILimitFilter, ISortingFilter<ActivitySortField>
{
    public static readonly SortOptions<ActivitySortField> DefaultSorting = new(
        ActivitySortField.CreatedAt,
        SortOrder.Descending);

    private readonly SortOptions<ActivitySortField> _sorting = DefaultSorting;
    
    public Limit Limit { get; init; }

    public SortOptions<ActivitySortField> Sorting
    {
        get => _sorting;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            _sorting = value;
        }
    }
    
    public UserIdentifier UserId { get; }
    public string SearchText { get; }

    public ActivitiesFilter(UserIdentifier userId, string searchText)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        UserId = userId;
        SearchText = searchText;
        
        Limit = Limit.Default;
    }
}