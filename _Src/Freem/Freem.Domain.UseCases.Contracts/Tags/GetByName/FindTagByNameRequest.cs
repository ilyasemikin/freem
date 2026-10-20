using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.GetByName;

public sealed class FindTagByNameRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.FindByName;

    public string SearchText { get; }
    
    public FindTagByNameRequest(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);
        
        SearchText = searchText;
    }
}