using Freem.Entities.Validation.Rules.Abstractions;
using Freem.Entities.Validation.Rules.Implementations;

namespace Freem.Entities.Validation;

public sealed class ValidatorBuilder<T>
{
    private readonly List<IValidationRule<T>> _rules = [];

    public ValidatorBuilder<T> ValueMust(Predicate<T> predicate, Func<T, string> errorMessageFactory)
    {
        var rule = new ValidationRuleValueMust<T>(value => predicate(value), value => errorMessageFactory(value));
        _rules.Add(rule);

        return this;
    }
    
    public Validator<T> Build()
    {
        return new Validator<T>(_rules);
    }
}