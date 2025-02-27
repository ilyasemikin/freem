using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Tags;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.GetByName;

public sealed class GetTagByNameResponse : IResponse<GetTagByNameErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tag))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Tag? Tag { get; }
    public Error<GetTagByNameErrorCode>? Error { get; }

    private GetTagByNameResponse(Tag? tag = null, Error<GetTagByNameErrorCode>? error = null)
    {
        Success = tag is not null;
        Tag = tag;
        Error = error;
    }
    
    public static GetTagByNameResponse CreateSuccess(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        return new GetTagByNameResponse(tag);
    }

    public static GetTagByNameResponse CreateFailure(GetTagByNameErrorCode code, string? message = null)
    {
        var error = new Error<GetTagByNameErrorCode>(code, message);
        return new GetTagByNameResponse(error: error);
    }

    public static GetTagByNameResponse Create(SearchEntityResult<Tag> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetTagByNameErrorCode.TagNotFound);
    }
}