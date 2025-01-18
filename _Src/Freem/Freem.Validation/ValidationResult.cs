using System.Diagnostics.CodeAnalysis;
using Freem.Validation.Exceptions;

namespace Freem.Validation;

public sealed class ValidationResult
{
    private static readonly ValidationResult CachedSuccess = new(true);

    [MemberNotNullWhen(true, nameof(Errors))]
    public bool Valid { get; }

    public IReadOnlyList<ValidationError>? Errors { get; }
    
    private ValidationResult(bool valid, IEnumerable<ValidationError>? errors = null)
    {
        Valid = valid;
        Errors = errors?.ToArray();
    }

    public void ThrowIfInvalid()
    {
        if (!Valid)
            throw new ValidationFailedException();
    }
    
    public static ValidationResult Success()
    {
        return CachedSuccess;
    }

    public static ValidationResult Failed(IEnumerable<ValidationError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        
        return new ValidationResult(false, errors);
    }
}