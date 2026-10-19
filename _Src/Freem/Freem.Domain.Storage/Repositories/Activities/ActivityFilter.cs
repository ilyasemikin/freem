using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Domain.Storage.Repositories.Activities;

public sealed class ActivityFilter : ILimitFilter, ISortingFilter<ActivitySortField>
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

    public ActivityFilter(UserIdentifier userId, string searchText)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        UserId = userId;
        SearchText = searchText;
        
        Limit = Limit.Default;
    }
}