using System.Text.Json;
using Confluent.Kafka;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Events.Consumer.Kafka.Models;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Users.Identifiers;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Consumer.Kafka.Implementations;

public sealed class KafkaEventConsumer
{
    private readonly KafkaConsumerConfiguration _configuration;
    private readonly EventConsumersExecutor _executor;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<KafkaEventConsumer> _logger;

    public KafkaEventConsumer(
        KafkaConsumerConfiguration configuration,
        EventJsonConverter converter, 
        EventConsumersExecutor executor, 
        ILogger<KafkaEventConsumer> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(logger);
        
        _configuration = configuration;
        _executor = executor;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.Converters.Add(converter);
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        using var consumer = CreateConsumer();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAsync(consumer, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }

    private IConsumer<Ignore, string> CreateConsumer()
    {
        var config = new ConsumerConfig
        {
            GroupId = _configuration.GroupId,
            BootstrapServers = _configuration.BootstrapServers,
        };
        
        var builder = new ConsumerBuilder<Ignore, string>(config);
        
        var consumer = builder.Build();
        foreach (var topic in _configuration.Topics)
            consumer.Subscribe(topic);

        return consumer;
    }

    private async Task ProcessAsync(IConsumer<Ignore, string> consumer, CancellationToken cancellationToken)
    {
        var cr = consumer.Consume(cancellationToken);
        
        var message = cr.Message;
        var @event = JsonSerializer.Deserialize<IEntityEvent<IEntityIdentifier, UserIdentifier>>(message.Value, _jsonOptions);
        if (@event is null)
        {
            _logger.LogError("");
            return;
        }
        
        await _executor.ExecuteAsync(@event, cancellationToken);
    }
}