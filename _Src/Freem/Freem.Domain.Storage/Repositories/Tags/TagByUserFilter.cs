using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Domain.Storage.Repositories.Tags;

public sealed class TagByUserFilter : ILimitFilter, IOffsetFilter, ISortingFilter<TagSortField>
{
    public static readonly SortOptions<TagSortField> DefaultSorting = new(TagSortField.Name, SortOrder.Ascending);

    private readonly SortOptions<TagSortField> _sorting = DefaultSorting;
    
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

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

    public TagByUserFilter(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}