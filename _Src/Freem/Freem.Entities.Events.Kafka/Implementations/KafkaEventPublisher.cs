using System.Text.Json;
using Confluent.Kafka;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Kafka.DependencyInjection;
using Freem.Entities.Events.Production.Abstractions;
using Freem.Entities.Users.Identifiers;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Kafka.Implementations;

public sealed class KafkaEventPublisher : IEventPublisher
{
    private readonly KafkaConfiguration _configuration;
    private readonly EventTopicResolver _topicResolver;
    private readonly JsonSerializerOptions _options;
    private readonly ILogger _logger;

    public KafkaEventPublisher(
        KafkaConfiguration configuration,
        EventTopicResolver topicResolver,
        EventJsonConverter converter,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(topicResolver);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(logger);
        
        _configuration = configuration;
        _topicResolver = topicResolver;
        _logger = logger;

        _options = new JsonSerializerOptions();
        _options.Converters.Add(converter);
    }

    public async Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        CancellationToken cancellationToken = default)
    {
        var config = new ProducerConfig
        { 
            BootstrapServers = _configuration.BootstrapServers,
        };

        var builder = new ProducerBuilder<Null, string>(config);
        using var producer = builder.Build();

        var topic = _topicResolver.Resolve(@event);
        var json = JsonSerializer.Serialize(@event, _options);
        
        try
        {
            var message = new Message<Null, string>()
            {
                Value = json
            };
            
            await producer.ProduceAsync(topic, message, cancellationToken);
        }
        catch (ProduceException<Null, string> e)
        {
            var type = @event.GetType();
            var name = type.Name;
            _logger.LogError("Can't publish event \"{Name}\". Reason: {ExceptionMessage}", name, e.Message);
        }
    }
}