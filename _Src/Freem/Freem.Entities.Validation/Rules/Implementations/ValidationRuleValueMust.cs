using Freem.Entities.Validation.Rules.Abstractions;
using Freem.Entities.Validation.Rules.Models;

namespace Freem.Entities.Validation.Rules.Implementations;

public sealed class ValidationRuleValueMust<T> : IValidationRule<T>
{
    public delegate bool Predicate(T value);
    public delegate string ErrorMessageFactory(T value);
    
    private readonly Predicate _predicate;
    private readonly ErrorMessageFactory _errorMessageFactory;

    public ValidationRuleValueMust(Predicate predicate, ErrorMessageFactory errorMessageFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessageFactory);
        
        _predicate = predicate;
        _errorMessageFactory = errorMessageFactory;
    }

    public ValidationRuleResult Validate(T value)
    {
        var success = _predicate(value);
        
        if (success)
            return ValidationRuleResult.Success();

        var message = _errorMessageFactory(value);
        var error = new ValidationError(message);
        return ValidationRuleResult.Failed(error);
    }
}