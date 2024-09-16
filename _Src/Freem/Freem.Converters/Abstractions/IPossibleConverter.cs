using System.Diagnostics.CodeAnalysis;

namespace Freem.Converters.Abstractions;

public interface IPossibleConverter<in TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    bool TryConvert(TInput input, [NotNullWhen(true)] out TOutput? output);
}

public interface IPossibleConverter<in TInput1, in TInput2, TOutput>
    where TInput1 : class
    where TInput2 : class
    where TOutput : class
{
    bool TryConvert(TInput1 input1, TInput2 input2, [NotNullWhen(true)] out TOutput? output);
}