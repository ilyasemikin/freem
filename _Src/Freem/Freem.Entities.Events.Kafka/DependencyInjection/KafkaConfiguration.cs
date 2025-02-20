namespace Freem.Entities.Events.Kafka.DependencyInjection;

public class KafkaConfiguration
{
    public string BootstrapServers { get; }
    
    public KafkaConfiguration(string bootstrapServers)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bootstrapServers);
        
        BootstrapServers = bootstrapServers;
    }
}