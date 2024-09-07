namespace Freem.Converters.Abstractions;

public interface IConverter<in TInput, out TOutput>
    where TInput : class
    where TOutput : class
{
    TOutput Convert(TInput input);
}