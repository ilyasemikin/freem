using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;
using Freem.Entities.Storage.Abstractions.Models.Sorting;
using Freem.Entities.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public class TagsFilter : ILimitFilter, ISortingFilter<TagSortField>
{
    public static readonly SortOptions<TagSortField> DefaultSorting = new(
        TagSortField.Name,
        SortOrder.Descending);

    private readonly SortOptions<TagSortField> _sorting = DefaultSorting;
    
    public Limit Limit { get; init; }

    public SortOptions<TagSortField> Sorting
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

    public TagsFilter(UserIdentifier userId, string searchText)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        UserId = userId;
        SearchText = searchText;
        
        Limit = Limit.Default;
    }
}