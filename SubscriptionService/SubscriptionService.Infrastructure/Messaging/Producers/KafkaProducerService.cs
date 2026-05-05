using System.Text.Json;
using SubscriptionService.Domain.Events;
using SubscriptionService.Domain.Interfaces;
using Confluent.Kafka;

namespace SubscriptionService.Infrastructure.Messaging.Producers;

public class KafkaProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    
    public async Task PublishSendAsync(string topic, Object evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
    }

    
}