namespace Freem.Entities.Validation;

public sealed class ValidationError
{
    public string Message { get; }

    public ValidationError(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        
        Message = message;
    }
}