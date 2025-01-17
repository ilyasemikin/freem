using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Activities.Archive;

public sealed class ArchiveActivityResponse : IResponse<ArchiveActivityErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public Error<ArchiveActivityErrorCode>? Error { get; }

    private ArchiveActivityResponse(Error<ArchiveActivityErrorCode>? error = null)
    {
        Success = error is null;
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