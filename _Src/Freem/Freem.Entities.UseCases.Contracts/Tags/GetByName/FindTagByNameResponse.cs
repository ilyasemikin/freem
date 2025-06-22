using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.GetByName;

public sealed class FindTagByNameResponse : IResponse<FindTagByNameErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tags))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public IReadOnlyList<Tag>? Tags { get; }
    public TotalCount? TotalCount { get; }

    public Error<FindTagByNameErrorCode>? Error { get; }

    private FindTagByNameResponse(
        IReadOnlyList<Tag>? tags = null,
        TotalCount? totalCount = null,
        Error<FindTagByNameErrorCode>? error = null)
    {
        Success = tags is not null;
        
        Tags = tags;
        TotalCount = totalCount;
        Error = error;
    }
    
    public static FindTagByNameResponse CreateSuccess(IReadOnlyList<Tag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(totalCount);
        ArgumentNullException.ThrowIfNull(tags);

        return new FindTagByNameResponse(tags, totalCount);
    }
}