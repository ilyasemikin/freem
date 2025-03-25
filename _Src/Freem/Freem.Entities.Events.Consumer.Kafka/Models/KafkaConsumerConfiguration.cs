namespace Freem.Entities.Events.Consumer.Kafka.Models;

public sealed class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; }
    public string GroupId { get; }
    
    public IReadOnlyList<string> Topics { get; }

    public KafkaConsumerConfiguration(string bootstrapServers, string groupId, IEnumerable<string> topics)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bootstrapServers);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentNullException.ThrowIfNull(topics);
        
        BootstrapServers = bootstrapServers;
        GroupId = groupId;
        Topics = topics.ToArray();
        
        if (Topics.Count == 0)
            throw new ArgumentException("At least one topic must be specified.", nameof(topics));
    }
}