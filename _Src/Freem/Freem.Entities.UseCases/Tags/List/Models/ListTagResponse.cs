using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Abstractions.Models.Errors;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Tags.List.Models;

public sealed class ListTagResponse
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