using Freem.Entities.Validation.Rules.Abstractions;

namespace Freem.Entities.Validation;

public class Validator<T>
{
    private readonly IReadOnlyList<IValidationRule<T>> _rules;
    
    public Validator(IEnumerable<IValidationRule<T>> rules)
    {
        _rules = rules.ToArray();
    }
    
    public ValidationResult Validate(T value)
    {
        var errors = new List<ValidationError>();
        foreach (var rule in _rules)
        {
            var result = rule.Validate(value);
            if (!result.Valid)
                errors.Add(result.Error);
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failed(errors);
    }
}