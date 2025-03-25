using System.Diagnostics.CodeAnalysis;

namespace Freem.Validation.Rules.Models;

public sealed class ValidationRuleResult
{
    private static readonly ValidationRuleResult CachedSuccess = new();
    
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Valid { get; }
    
    public ValidationError? Error { get; }

    private ValidationRuleResult(ValidationError? error = default)
    {
        Valid = error is null;
        Error = error;
    }

    public static ValidationRuleResult Success()
    {
        return CachedSuccess;
    }

    public static ValidationRuleResult Failed(ValidationError error)
    {
        return new ValidationRuleResult(error);
    }
}