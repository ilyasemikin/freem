namespace Freem.UseCases.Contracts.Abstractions.Errors;

public sealed class Error<TCode>
    where TCode : struct, Enum
{
    public TCode Code { get; }
    
    public string? Message { get; }
    public IReadOnlyDictionary<string, string>? Properties { get; }

    public Error(TCode code, string? message = null, IReadOnlyDictionary<string, string>? properties = null)
    {
        if (!Enum.IsDefined(code))
            throw new ArgumentOutOfRangeException(nameof(code), code, null);
        
        Code = code;
        Message = message;
        Properties = properties;
    }
}