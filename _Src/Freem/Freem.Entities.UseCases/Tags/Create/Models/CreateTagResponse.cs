using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Tags.Create.Models;

public sealed class CreateTagResponse
{
    [MemberNotNullWhen(true, nameof(Tag))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Tag? Tag { get; }
    public Error<CreateTagErrorCode>? Error { get; }

    private CreateTagResponse(Tag? tag = null, Error<CreateTagErrorCode>? error = null)
    {
        Success = tag is not null;
        Tag = tag;
        Error = error;
    }

    public static CreateTagResponse CreateSuccess(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);
        
        return new CreateTagResponse(tag);
    }

    public static CreateTagResponse CreateFailure(CreateTagErrorCode code, string? message = null)
    {
        var error = new Error<CreateTagErrorCode>(code, message);
        return new CreateTagResponse(error: error);
    }
}