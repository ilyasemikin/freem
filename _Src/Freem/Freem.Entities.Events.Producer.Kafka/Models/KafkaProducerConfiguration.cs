namespace Freem.Entities.Events.Producer.Kafka.Models;

public class KafkaProducerConfiguration
{
    public string BootstrapServers { get; }
    
    public KafkaProducerConfiguration(string bootstrapServers)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bootstrapServers);
        
        BootstrapServers = bootstrapServers;
    }
}