using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Users.Identifiers;
using Freem.Sorting;

namespace Freem.Domain.Storage.Repositories.Tags;

public class TagFilter : ILimitFilter, ISortingFilter<TagSortField>
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

    public TagFilter(UserIdentifier userId, string searchText)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        UserId = userId;
        SearchText = searchText;
        
        Limit = Limit.Default;
    }
}