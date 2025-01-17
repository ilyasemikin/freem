using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Abstractions;

public interface IResponse<TErrorCode>
    where TErrorCode : struct, Enum
{
    [MemberNotNullWhen(false, nameof(Error))]
    bool Success { get; }
    
    Error<TErrorCode>? Error { get; } 
}