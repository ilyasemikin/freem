namespace Freem.Validation.Extensions;

public static class ValidatorBuilderExtensions
{
    public static ValidatorBuilder<T> ValueMust<T>(
        this ValidatorBuilder<T> builder, 
        Predicate<T> predicate, string errorMessage)
    {
        return builder.ValueMust(predicate, _ => errorMessage);
    }
}