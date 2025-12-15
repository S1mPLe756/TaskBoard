using System.Text.Json;
using Confluent.Kafka;
using FileService.Domain.Events;
using FileService.Domain.Interfaces;

namespace FileService.Infrastructure.Messaging;

public class FileKafkaProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public FileKafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    public async Task ProduceAsync(string topic, FileUploadEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
    }
}