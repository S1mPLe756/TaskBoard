using System.Text.Json;
using Confluent.Kafka;
using Organization.Domain.Events;
using Organization.Domain.Interfaces;

namespace Organization.Infrastructure.Messaging;

public class KafkaProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishInvitationSendAsync(InvitationSendEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync("notification-email-send", new Message<Null, string> { Value = json });
    }

}