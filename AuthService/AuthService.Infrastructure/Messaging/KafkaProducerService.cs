using System.Text.Json;
using AuthService.Domain.Events;
using AuthService.Domain.Interfaces;
using Confluent.Kafka;

namespace AuthService.Infrastructure.Messaging.Producers;

public class KafkaProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishUserRegisteredAsync(UserRegisteredEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync("user-registered", new Message<Null, string> { Value = json });
    }

}