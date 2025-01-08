using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Tags.Get.Models;

public sealed class GetTagResponse
{
    [MemberNotNullWhen(true, nameof(Tag))]
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

    internal static GetTagResponse Create(SearchEntityResult<Tag> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetTagErrorCode.TagNotFound);
    }
}