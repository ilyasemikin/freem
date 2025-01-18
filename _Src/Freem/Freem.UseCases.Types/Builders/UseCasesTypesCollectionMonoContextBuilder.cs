using System.Diagnostics.CodeAnalysis;

namespace Freem.UseCases.Types.Builders;

public sealed class UseCasesTypesCollectionMonoContextBuilder<TContext>
    where TContext : notnull
{
    private readonly IList<Type> _types;
    
    private readonly IDictionary<Type, UseCasesTypesCollection.UseCaseDescriptor> _descriptorsByType;
    
    public UseCasesTypesCollectionMonoContextBuilder()
    {
        _types = new List<Type>();
        _descriptorsByType = new Dictionary<Type, UseCasesTypesCollection.UseCaseDescriptor>();
    }
    
    public UseCasesTypesCollectionMonoContextBuilder<TContext> Add<TUseCase>()
    {
        var type = typeof(TUseCase);
        if (!UseCasesTypesCollection.UseCaseDescriptor.TryCreate(type, out var descriptor))
            throw new InvalidOperationException();
        if (descriptor.Arguments.ContextType != typeof(TContext))
            throw new InvalidOperationException();
        
        if (_descriptorsByType.ContainsKey(type))
            throw new InvalidOperationException();
        
        _types.Add(type);
        _descriptorsByType.Add(type, descriptor);
        
        return this;
    }

    public bool TryGetDescriptor(Type useCaseType, [NotNullWhen(true)] out UseCasesTypesCollection.UseCaseDescriptor? descriptor)
    {
        return _descriptorsByType.TryGetValue(useCaseType, out descriptor);
    }
    
    public UseCasesTypesCollection Build()
    {
        return new UseCasesTypesCollection(_types);
    }
}