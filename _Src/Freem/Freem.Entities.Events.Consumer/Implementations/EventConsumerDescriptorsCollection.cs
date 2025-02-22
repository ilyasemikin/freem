using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Reflection.Extensions;

namespace Freem.Entities.Events.Consumer.Implementations;

public sealed class EventConsumerDescriptorsCollection : IEnumerable<EventConsumerDescriptorsCollection.EventConsumerDescriptor>
{
    private readonly IReadOnlyDictionary<Type, IReadOnlyList<EventConsumerDescriptor>> _consumers;
    private readonly IReadOnlyList<EventConsumerDescriptor> _descriptors;

    private EventConsumerDescriptorsCollection(IReadOnlyDictionary<Type, IReadOnlyList<EventConsumerDescriptor>> consumers)
    {
        _consumers = consumers;
        _descriptors = _consumers.Values
            .SelectMany(value => value)
            .ToArray();
    }

    public bool TryGet(Type eventType, [NotNullWhen(true)] out IEnumerable<EventConsumerDescriptor>? descriptors)
    {
        _consumers.TryGetValue(eventType, out var consumers);
        descriptors = consumers;
        return descriptors is not null;
    }
    
    public IEnumerator<EventConsumerDescriptor> GetEnumerator()
    {
        return _descriptors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class EventConsumerDescriptor
    {
        private readonly MethodInfo _executeMethod;
        
        public Type EventType { get; }
        public Type ConsumerType { get; }

        public EventConsumerDescriptor(Type eventType, Type consumerType)
        {
            ArgumentNullException.ThrowIfNull(eventType);
            ArgumentNullException.ThrowIfNull(consumerType);
            
            EventType = eventType;
            ConsumerType = consumerType;
            
            const string methodName = nameof(IEventConsumer<IEntityEvent<IEntityIdentifier, UserIdentifier>>.ExecuteAsync);
            var @interface = typeof(IEventConsumer<>).MakeGenericType(EventType);
            var method = @interface.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            _executeMethod = method ?? throw new InvalidOperationException($"Method \"{@interface.Name}.{methodName}\" not found.");
        }

        public async Task CallExecuteAsync(
            object consumer, IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
            CancellationToken cancellationToken = default)
        {
            var task = _executeMethod.Invoke(consumer, [@event, cancellationToken]) as Task;
            if (task is null)
                throw new InvalidOperationException();

            await task;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(EventType, ConsumerType);
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is null) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != GetType()) 
                return false;
            
            var other = (EventConsumerDescriptor)obj;
            return EventType == other.EventType && ConsumerType == other.ConsumerType;
        }

    }
    
    public sealed class Builder
    {
        private static readonly Type EventConsumerGenericType = typeof(IEventConsumer<>);
        
        private readonly Dictionary<Type, List<EventConsumerDescriptor>> _consumers = [];
        private readonly HashSet<EventConsumerDescriptor> _unique = [];

        public bool TryAdd(Type consumerType)
        {
            if (consumerType.IsAbstract)
                return false;

            var someAdded = false;
            var interfaces = consumerType.GetGenericInterfaceImplementations(EventConsumerGenericType);
            foreach (var @interface in interfaces)
            {
                var eventType = @interface.GetRequiredGenericArgument(0);
                
                var descriptor = new EventConsumerDescriptor(eventType, consumerType);
                if (!_unique.Contains(descriptor))
                    continue;
                
                Add(descriptor);
                someAdded = true;
            }
            
            return someAdded;
        }
        
        public bool TryAdd(Type eventType, Type consumerType)
        {
            if (consumerType.IsAbstract)
                return false;
            
            var interfaceType = typeof(IEventConsumer<>).MakeGenericType(eventType);
            if (!consumerType.IsAssignableTo(interfaceType))
                return false;
            
            var descriptor = new EventConsumerDescriptor(eventType, consumerType);
            if (_unique.Contains(descriptor))
                return false;
            
            Add(descriptor);
            return true;
        }
        
        public bool TryAdd<TEvent, TEventConsumer>()
            where TEvent : IEntityEvent<IEntityIdentifier, UserIdentifier>
            where TEventConsumer : IEventConsumer<TEvent>
        {
            var consumerType = typeof(TEventConsumer);
            if (consumerType.IsAbstract)
                return false;
         
            var eventType = typeof(TEvent);
            var descriptor = new EventConsumerDescriptor(eventType, consumerType);
            if (_unique.Contains(descriptor))
                return false;
            
            Add(descriptor);
            return true;
        }

        public EventConsumerDescriptorsCollection Build()
        {
            var consumers = _consumers.ToDictionary(p => p.Key, IReadOnlyList<EventConsumerDescriptor> (p) => p.Value);
            return new EventConsumerDescriptorsCollection(consumers);
        }

        private void Add(EventConsumerDescriptor descriptor)
        {
            if (!_consumers.TryGetValue(descriptor.EventType, out var consumers))
            {
                consumers = [];
                _consumers.Add(descriptor.EventType, consumers);
            }
            
            consumers.Add(descriptor);
            _unique.Add(descriptor);
        }
    }
}