using Freem.Converters.Abstractions;

namespace Freem.Converters.Collections.Builders;

public sealed class ConvertersCollectionBuilder<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    private readonly Dictionary<Type, object> _converters;

    public int Count => _converters.Count;
    
    public ConvertersCollectionBuilder()
    {
        _converters = new Dictionary<Type, object>();
    }

    public bool TryAdd<TAddedInput, TAddedOutput>(IConverter<TAddedInput, TAddedOutput> converter)
        where TAddedInput : class, TInput
        where TAddedOutput : class, TOutput
    {
        return _converters.TryAdd(typeof(TAddedInput), converter);
    }

    public bool TryAdd<TAddedInput, TAddedOutput>(IPossibleConverter<TAddedInput, TAddedOutput> converter)
        where TAddedInput : class, TInput
        where TAddedOutput : class, TOutput
    {
        return _converters.TryAdd(typeof(TAddedInput), converter);
    }

    public ConvertersCollectionBuilder<TInput, TOutput> Add<TAddedInput, TAddedOutput>(
        IConverter<TAddedInput, TAddedOutput> converter)
        where TAddedInput : class, TInput
        where TAddedOutput : class, TOutput
    {
        if (!TryAdd(converter))
            throw new InvalidOperationException("The converter with this input type has already been added");

        return this;
    }

    public ConvertersCollectionBuilder<TInput, TOutput> Add<TAddedInput, TAddedOutput>(
        IPossibleConverter<TAddedInput, TAddedOutput> converter)
        where TAddedInput : class, TInput
        where TAddedOutput : class, TOutput
    {
        if (!TryAdd(converter))
            throw new InvalidOperationException("The converter with this input type has already been added");

        return this;
    }

    public ConvertersCollection<TInput, TOutput> Build()
    {
        return new ConvertersCollection<TInput, TOutput>(_converters.Values);
    }
}