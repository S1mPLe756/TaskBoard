using System.Text.Json;
using CardService.Domain.Events;
using CardService.Domain.Interfaces;
using Confluent.Kafka;

namespace CardService.Infrastructure.Messaging;

public class KafkaProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishCardCreatedSendAsync(CardCreatedEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync("card-created", new Message<Null, string> { Value = json });
    }

}