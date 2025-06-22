using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Activities.Update;

public sealed class UpdateActivityResponse : IResponse<UpdateActivityErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UpdateActivityErrorCode>? Error { get; }

    private UpdateActivityResponse(Error<UpdateActivityErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateActivityResponse CreateSuccess()
    {
        return new UpdateActivityResponse();
    }

    public static UpdateActivityResponse CreateFailure(UpdateActivityErrorCode code)
    {
        var error = new Error<UpdateActivityErrorCode>(code);
        return new UpdateActivityResponse(error);
    }
}