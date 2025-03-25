using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Activities.Unarchive;

public sealed class UnarchiveActivityResponse : IResponse<UnarchiveActivityErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UnarchiveActivityErrorCode>? Error { get; }

    private UnarchiveActivityResponse(Error<UnarchiveActivityErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UnarchiveActivityResponse CreateSuccess()
    {
        return new UnarchiveActivityResponse();
    }

    public static UnarchiveActivityResponse CreateFailure(UnarchiveActivityErrorCode code, string? message = null)
    {
        var error = new Error<UnarchiveActivityErrorCode>(code, message);
        return new UnarchiveActivityResponse(error);
    }
}