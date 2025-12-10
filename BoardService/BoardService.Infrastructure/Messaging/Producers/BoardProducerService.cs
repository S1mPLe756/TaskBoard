using System.Text.Json;
using BoardService.Domain.Events;
using BoardService.Domain.Interfaces;
using BoardService.Infrastructure.Settings;
using Confluent.Kafka;

namespace BoardService.Infrastructure.Messaging.Producers;

public class BoardProducerService : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public BoardProducerService(KafkaSettings kafkaSettings)
    {
        var config = new ProducerConfig { BootstrapServers = kafkaSettings.BootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishBoardDeleteSendAsync(BoardDeleteEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync("board-delete-send", new Message<Null, string> { Value = json });
    }

    public async Task PublishColumnDeleteSendAsync(ColumnDeleteEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _producer.ProduceAsync("column-delete-send", new Message<Null, string> { Value = json });
    }
}