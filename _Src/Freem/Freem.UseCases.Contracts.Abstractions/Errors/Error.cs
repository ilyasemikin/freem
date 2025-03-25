using Freem.Enums.Exceptions;

namespace Freem.UseCases.Contracts.Abstractions.Errors;

public sealed class Error<TCode>
    where TCode : struct, Enum
{
    public TCode Code { get; }
    public string? AdditionalMessage { get; }

    public Error(TCode code, string? additionalMessage = null)
    {
        InvalidEnumValueException<TCode>.ThrowIfValueInvalid(code);
        
        Code = code;
        AdditionalMessage = additionalMessage;
    }
}