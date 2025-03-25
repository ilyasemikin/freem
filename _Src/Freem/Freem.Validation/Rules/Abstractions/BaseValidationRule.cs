using Freem.Validation.Rules.Models;

namespace Freem.Validation.Rules.Abstractions;

public interface IValidationRule<in T>
{
    ValidationRuleResult Validate(T value);
}