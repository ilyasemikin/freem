using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class TagsByUserFilter : ILimitFilter, IOffsetFilter, ISortingFilter<TagSortField>
{
    public static readonly SortOptions<TagSortField> DefaultSorting = new(TagSortField.Name, SortOrder.Ascending);

    private readonly SortOptions<TagSortField> _sorting = DefaultSorting;
    
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }

    public SortOptions<TagSortField> Sorting
    {
        get => _sorting;
        init
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sorting = value;
        }
    }
    
    public UserIdentifier UserId { get; }

    public TagsByUserFilter(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}