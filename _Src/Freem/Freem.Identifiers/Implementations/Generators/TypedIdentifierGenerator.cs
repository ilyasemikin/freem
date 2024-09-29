using System.Diagnostics.CodeAnalysis;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Reflection.Extensions;

namespace Freem.Identifiers.Implementations.Generators;

public sealed class TypedIdentifierGenerator
{
    private readonly Dictionary<Type, IIdentifierGenerator<IIdentifier>> _generators;

    public TypedIdentifierGenerator(IEnumerable<IIdentifierGenerator<IIdentifier>> generators)
    {
        var dictionary = new Dictionary<Type, IIdentifierGenerator<IIdentifier>>();
        foreach (var generator in generators)
        {
            var type = generator.GetType();
            var @interface = type.GetRequiredInterface("IIdentifierGenerator`1");
            var identifierType = @interface.GetRequiredGenericArgument(0);

            if (!dictionary.TryAdd(identifierType, generator))
                throw new InvalidOperationException($"\"{identifierType}\" already added");
        }

        if (dictionary.Count == 0)
            throw new InvalidOperationException($"\"{nameof(generators)}\" is empty");
        
        _generators = dictionary;
    }

    public TypedIdentifierGenerator(params IIdentifierGenerator<IIdentifier>[] generators)
        : this(generators.AsEnumerable())
    {
    }
    
    public bool TryGenerate<TIdentifier>([NotNullWhen(true)] out TIdentifier? identifier)
        where TIdentifier : class, IIdentifier
    {
        identifier = default;
        if (!_generators.TryGetValue(typeof(TIdentifier), out var generator))
            return false;

        identifier = generator.Generate() as TIdentifier;
        return identifier is not null;
    }

    public TIdentifier Generate<TIdentifier>()
        where TIdentifier : class, IIdentifier
    {
        if (!TryGenerate<TIdentifier>(out var identifier))
            throw new InvalidOperationException($"Can't generate \"{nameof(TIdentifier)}\"");

        return identifier;
    }
}