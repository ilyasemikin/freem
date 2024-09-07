using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Reflection.Extensions;

namespace Freem.Converters.Collections;

public sealed class ConvertersCollection<TInput, TOutput>
    where TInput : class 
    where TOutput : class
{
    private const string ConverterInterfaceName = "IConverter`2";
    private const string PossibleConverterInterfaceName = "IPossibleConverter`2";
    
    private delegate TOutput? Converter(TInput input);
    
    private readonly FrozenDictionary<Type, Converter> _converters;

    public int Count => _converters.Count;

    internal ConvertersCollection(IEnumerable<object> converters)
    {
        ArgumentNullException.ThrowIfNull(converters);
        
        _converters = BuildConverters(converters);
    }

    public bool TryConvert(TInput input, [NotNullWhen(true)] out TOutput? output)
    {
        output = default;

        var type = input.GetType();
        if (!_converters.TryGetValue(type, out var converter))
            return false;

        output = converter(input);
        return output is not null;
    }

    private static FrozenDictionary<Type, Converter> BuildConverters(IEnumerable<object> converters)
    {
        var dictionary = new Dictionary<Type, Converter>();

        foreach (var converter in converters)
        {
            var type = converter.GetType();

            Converter convert;
            if (type.TryGetInterface(ConverterInterfaceName, out var @interface))
                convert = BuildConverter(converter, type);
            else if (type.TryGetInterface(PossibleConverterInterfaceName, out @interface))
                convert = BuildPossibleConverter(converter, @interface);
            else
                throw new InvalidOperationException();

            var inputType = @interface.GetRequiredGenericArgument(0);

            dictionary.Add(inputType, convert);
        }
        
        return dictionary.ToFrozenDictionary();
        
        static Converter BuildConverter(object converter, Type type)
        {
            var method = type.GetRequiredMethod(nameof(IConverter<TInput, TOutput>.Convert));
            
            return input => method.Invoke(converter, [input]) as TOutput;
        }

        static Converter BuildPossibleConverter(object converter, Type type)
        {
            var method = type.GetRequiredMethod(nameof(IPossibleConverter<TInput, TOutput>.TryConvert));

            return input =>
            {
                var parameters = new object?[] { input, null };
                var result = (bool)method.Invoke(converter, parameters)!;
                return !result
                    ? null
                    : parameters[1] as TOutput;
            };
        }
    }
}