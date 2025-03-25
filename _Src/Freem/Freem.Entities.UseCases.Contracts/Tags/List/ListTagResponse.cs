using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.List;

public sealed class ListTagResponse : IResponse<ListTagErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tags))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<Tag>? Tags { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<ListTagErrorCode>? Error { get; }
    
    private ListTagResponse(
        IReadOnlyList<Tag>? tags = null, 
        TotalCount? totalCount = null, 
        Error<ListTagErrorCode>? error = null)
    {
        Success = error is null;
        Tags = tags;
        TotalCount = totalCount;
        Error = error;
    }

    public static ListTagResponse CreateSuccess(IReadOnlyList<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);

        return new ListTagResponse(tags, totalCount);
    }

    public static ListTagResponse CreateSuccess(ListTagErrorCode code, string? message = null)
    {
        var error = new Error<ListTagErrorCode>(code, message);
        return new ListTagResponse(error: error);
    }
}