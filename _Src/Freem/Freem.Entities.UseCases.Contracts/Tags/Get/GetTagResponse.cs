using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Tags;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.Get;

public sealed class GetTagResponse : IResponse<GetTagErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tag))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Tag? Tag { get; }
    public Error<GetTagErrorCode>? Error { get; }

    private GetTagResponse(Tag? tag = null, Error<GetTagErrorCode>? error = null)
    {
        Success = tag is not null;
        Tag = tag;
        Error = error;
    }

    public static GetTagResponse CreateSuccess(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        return new GetTagResponse(tag);
    }

    public static GetTagResponse CreateFailure(GetTagErrorCode code, string? message = null)
    {
        var error = new Error<GetTagErrorCode>(code, message);
        return new GetTagResponse(error: error);
    }

    public static GetTagResponse Create(SearchEntityResult<Tag> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetTagErrorCode.TagNotFound);
    }
}