using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Users;
using Freem.Sorting;

namespace Freem.Domain.Storage.Repositories.Activities;

public sealed class ActivityByUserFilter : ILimitFilter, IOffsetFilter, ISortingFilter<ActivitySortField>
{
    public static readonly SortOptions<ActivitySortField> DefaultSorting = new(
        ActivitySortField.CreatedAt,
        SortOrder.Descending);

    private readonly SortOptions<ActivitySortField> _sorting = DefaultSorting;
    
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }
    
    public SortOptions<ActivitySortField> Sorting
    {
        get => _sorting;
        init
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sorting = value;
        }
    }
    
    public UserId UserId { get; }

    public ActivityByUserFilter(UserId userId) 
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}