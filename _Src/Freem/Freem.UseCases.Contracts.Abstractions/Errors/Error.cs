using Freem.Enums.Exceptions;

namespace Freem.UseCases.Contracts.Abstractions.Errors;

public sealed class Error<TCode>
    where TCode : struct, Enum
{
    public TCode Code { get; }
    
    public string? Message { get; }
    public IReadOnlyDictionary<string, string>? Properties { get; }

    public Error(TCode code, string? message = null, IReadOnlyDictionary<string, string>? properties = null)
    {
        InvalidEnumValueException<TCode>.ThrowIfValueInvalid(code);
        
        Code = code;
        Message = message;
        Properties = properties;
    }
}