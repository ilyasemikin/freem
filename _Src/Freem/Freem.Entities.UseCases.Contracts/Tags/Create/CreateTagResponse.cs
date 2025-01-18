using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Tags;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.Create;

public sealed class CreateTagResponse : IResponse<CreateTagErrorCode>
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