using Freem.Entities.Validation.Rules.Models;

namespace Freem.Entities.Validation.Rules.Abstractions;

public interface IValidationRule<in T>
{
    ValidationRuleResult Validate(T value);
}