using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.UseCases.Contracts.Abstractions;

public interface IResponse<TErrorCode>
    where TErrorCode : struct, Enum
{
    [MemberNotNullWhen(false, nameof(Error))]
    bool Success { get; }
    
    Error<TErrorCode>? Error { get; } 
}