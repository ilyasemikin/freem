using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class ActivitiesByUserFilter : ILimitFilter, IOffsetFilter, ISortingFilter<ActivitySortField>
{
    public static readonly SortOptions<ActivitySortField> DefaultSorting = new(
        ActivitySortField.CreatedAt,
        SortOrder.Descending);

    private readonly SortOptions<ActivitySortField> _sorting = DefaultSorting;
    
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
    
    public SortOptions<ActivitySortField> Sorting
    {
        get => _sorting;
        init
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sorting = value;
        }
    }
    
    public UserIdentifier UserId { get; }

    public ActivitiesByUserFilter(UserIdentifier userId) 
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}