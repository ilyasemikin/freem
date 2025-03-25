using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Freem.UseCases.Types;

public sealed class UseCasesTypesCollection
{
    private readonly IReadOnlyDictionary<ContextAndRequestTypes, UseCaseDescriptor> _descriptorsByContextAndRequest;
    private readonly IReadOnlyDictionary<ContextAndRequestTypes, UseCaseInvoker> _invokersByContextAndRequest;
    
    public UseCasesTypesCollection(IEnumerable<Type> types)
    {
        var descriptors = CreateDescriptors(types);
        var invokers = CreateInvokers(descriptors);

        _descriptorsByContextAndRequest = descriptors.ToDictionary(
            descriptor => new ContextAndRequestTypes(
                descriptor.Arguments.ContextType,
                descriptor.Arguments.RequestType));
        
        _invokersByContextAndRequest = invokers.ToDictionary(
            invoker => new ContextAndRequestTypes(
                invoker.Descriptor.Arguments.ContextType,
                invoker.Descriptor.Arguments.RequestType));
    }

    public bool TryGetDescriptor(Type context, Type request, [NotNullWhen(true)] out UseCaseDescriptor? descriptor)
    {
        var types = new ContextAndRequestTypes(context, request);
        return _descriptorsByContextAndRequest.TryGetValue(types, out descriptor);
    }
    
    public bool TryGetInvoker(Type context, Type request, [NotNullWhen(true)] out UseCaseInvoker? invoker)
    {
        var types = new ContextAndRequestTypes(context, request);
        return _invokersByContextAndRequest.TryGetValue(types, out invoker);
    }
    
    private static IReadOnlyList<UseCaseDescriptor> CreateDescriptors(IEnumerable<Type> types)
    {
        var descriptors = new List<UseCaseDescriptor>();
        foreach (var type in types)
        {
            if (!UseCaseDescriptor.TryCreate(type, out var descriptor))
                throw new InvalidOperationException($"Can't create descriptor for type \"{type}\"");
            
            descriptors.Add(descriptor);
        }

        return descriptors;
    }

    private static IReadOnlyList<UseCaseInvoker> CreateInvokers(IEnumerable<UseCaseDescriptor> descriptors)
    {
        var invokers = new List<UseCaseInvoker>();
        foreach (var descriptor in descriptors)
        {
            if (!UseCaseInvoker.TryCreate(descriptor, out var invoker))
                throw new InvalidOperationException($"Can't create invoker for type \"{descriptor.ImplementationType}\"");
            
            invokers.Add(invoker);
        }

        return invokers;
    }

    private record ContextAndRequestTypes(Type ContextType, Type RequestType);

    public sealed class UseCaseDescriptor
    {
        public UseCaseTypeArguments Arguments { get; }
        
        public Type AbstractionType { get; }
        public Type ImplementationType { get; }

        private UseCaseDescriptor(UseCaseTypeArguments arguments, Type abstractionType, Type implementationType)
        {
            Arguments = arguments;
            
            AbstractionType = abstractionType;
            ImplementationType = implementationType;
        }
        
        public static bool TryCreate(Type type, [NotNullWhen(true)] out UseCaseDescriptor? descriptor)
        {
            var interfaces = type.GetInterfaces();

            var useCaseType = interfaces.FirstOrDefault(@interface =>
                @interface.IsInterface && 
                @interface.FullName?.Contains("IUseCase") == true);

            if (useCaseType is null)
            {
                descriptor = null;
                return false;
            }

            var arguments = UseCaseTypeArguments.Create(useCaseType);
            
            descriptor = new UseCaseDescriptor(arguments, useCaseType, type);
            return true;
        }
    }

    public sealed class UseCaseTypeArguments
    {
        public Type ContextType { get; }
        public Type RequestType { get; }
        public Type ResponseType { get; }
        public Type ResponseErrorType { get; }

        private UseCaseTypeArguments(Type contextType, Type requestType, Type responseType, Type responseErrorType)
        {
            ContextType = contextType;
            RequestType = requestType;
            ResponseType = responseType;
            ResponseErrorType = responseErrorType;
        }

        internal static UseCaseTypeArguments Create(Type type)
        {
            var arguments = type.GetGenericArguments();
            
            var contextType = arguments[0];
            var requestType = arguments[1];
            var responseType = arguments[2];
            var responseErrorType = arguments[3];

            return new UseCaseTypeArguments(contextType, requestType, responseType, responseErrorType);
        }
    }

    public sealed class UseCaseInvoker
    {
        private readonly MethodInfo _method;
        private readonly PropertyInfo _resultProperty;

        public UseCaseDescriptor Descriptor { get; }
        
        private UseCaseInvoker(UseCaseDescriptor descriptor, MethodInfo method)
        {
            ArgumentNullException.ThrowIfNull(method);

            var returnType = method.ReturnType;
            var property = returnType.GetProperty(nameof(Task<object>.Result));

            _method = method;
            _resultProperty = property ?? throw new InvalidOperationException();
            
            Descriptor = descriptor;
        }

        public async Task<object> ExecuteAsync(
            object useCase, object context, object request, 
            CancellationToken cancellationToken = default)
        {
            var task = (Task?)_method.Invoke(useCase, [context, request, cancellationToken]);
            if (task is null)
            {
                throw new InvalidOperationException(
                    $"Method invocation invalid. Expected type is Task<object>");
            }

            await task;

            var result = _resultProperty.GetValue(task);
            if (result is null)
                throw new InvalidOperationException();

            return result;
        }

        internal static bool TryCreate(UseCaseDescriptor descriptor, [NotNullWhen(true)] out UseCaseInvoker? invoker)
        {
            var method = descriptor.ImplementationType.GetMethod("ExecuteAsync");
            if (method is null)
            {
                invoker = null;
                return false;
            }
            
            invoker = new UseCaseInvoker(descriptor, method);
            return true;
        }
    }
}