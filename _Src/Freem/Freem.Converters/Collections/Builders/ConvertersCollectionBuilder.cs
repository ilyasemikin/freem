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

public sealed class ConvertersCollectionBuilder<TInput1, TInput2, TOutput>
    where TInput1 : class
    where TInput2 : class
    where TOutput : class
{
    private readonly Dictionary<Tuple<Type, Type>, object> _converters;
    
    public int Count => _converters.Count;
    
    public ConvertersCollectionBuilder()
    {
        _converters = new Dictionary<Tuple<Type, Type>, object>();
    }
    
    public bool TryAdd<TAddedInput1, TAddedInput2, TAddedOutput>(
        IConverter<TAddedInput1, TAddedInput2, TAddedOutput> converter)
        where TAddedInput1 : class, TInput1
        where TAddedInput2 : class, TInput2
        where TAddedOutput : class, TOutput
    {
        var tuple = new Tuple<Type, Type>(typeof(TAddedInput1), typeof(TAddedInput2));
        return _converters.TryAdd(tuple, converter);
    }

    public bool TryAdd<TAddedInput1, TAddedInput2, TAddedOutput>(
        IPossibleConverter<TAddedInput1, TAddedInput2, TAddedOutput> converter)
        where TAddedInput1 : class, TInput1
        where TAddedInput2 : class, TInput2
        where TAddedOutput : class, TOutput
    {
        var tuple = new Tuple<Type, Type>(typeof(TAddedInput1), typeof(TAddedInput2));
        return _converters.TryAdd(tuple, converter);
    }

    public ConvertersCollectionBuilder<TInput1, TInput2, TOutput> Add<TAddedInput1, TAddedInput2, TAddedOutput>(
        IConverter<TAddedInput1, TAddedInput2, TAddedOutput> converter)
        where TAddedInput1 : class, TInput1
        where TAddedInput2 : class, TInput2
        where TAddedOutput : class, TOutput
    {
        if (!TryAdd(converter))
            throw new InvalidOperationException("The converter with this input type has already been added");

        return this;
    }

    public ConvertersCollectionBuilder<TInput1, TInput2, TOutput> Add<TAddedInput1, TAddedInput2, TAddedOutput>(
        IPossibleConverter<TAddedInput1, TAddedInput2, TAddedOutput> converter)
        where TAddedInput1 : class, TInput1
        where TAddedInput2 : class, TInput2
        where TAddedOutput : class, TOutput
    {
        if (!TryAdd(converter))
            throw new InvalidOperationException("The converter with this input type has already been added");

        return this;
    }

    public ConvertersCollection<TInput1, TInput2, TOutput> Build()
    {
        return new ConvertersCollection<TInput1, TInput2, TOutput>(_converters.Values);
    }
}