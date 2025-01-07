using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Activities.Archive.Models;

public sealed class ArchiveActivityResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    public Error<ArchiveActivityErrorCode>? Error { get; }

    private ArchiveActivityResponse(Error<ArchiveActivityErrorCode>? error = null)
    {
        IsSuccess = error is null;
        Error = error;
    }

    public static ArchiveActivityResponse CreateSuccess()
    {
        return new ArchiveActivityResponse();
    }

    public static ArchiveActivityResponse CreateFailure(ArchiveActivityErrorCode code, string? message = null)
    {
        var error = new Error<ArchiveActivityErrorCode>(code, message);
        return new ArchiveActivityResponse(error);
    }
}