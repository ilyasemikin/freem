using System.Diagnostics.CodeAnalysis;

namespace Freem.Converters.Abstractions;

public interface IPossibleConverter<in TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    bool TryConvert(TInput input, [NotNullWhen(true)] out TOutput? output);
}