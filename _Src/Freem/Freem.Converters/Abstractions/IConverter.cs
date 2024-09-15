namespace Freem.Converters.Abstractions;

public interface IConverter<in TInput, out TOutput>
    where TInput : class
    where TOutput : class
{
    TOutput Convert(TInput input);
}

public interface IConverter<in TInput1, in TInput2, out TOutput>
    where TInput1 : class
    where TInput2 : class
    where TOutput : class
{
    TOutput Convert(TInput1 input1, TInput2 input2);
}