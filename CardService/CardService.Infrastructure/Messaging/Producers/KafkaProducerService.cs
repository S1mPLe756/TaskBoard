using System.Text.Json;
using CardService.Domain.Events;
using CardService.Domain.Interfaces;
using Confluent.Kafka;

namespace CardService.Infrastructure.Messaging.Producers;

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

    public async Task PublishCardCreatedSendAsync(CardCreatedEvent evt)
    {
        await PublishSendAsync("card-created", evt);
    }

    public async Task PublishBoardCardsDeletedAsync(BoardDeletionCompletedEvent evt)
    {
        await PublishSendAsync("board-cards-deleted", evt);
    }

    public async Task PublishBoardCardsDeleteFailedAsync(BoardDeletionFailedEvent evt)
    {
        await PublishSendAsync("board-cards-delete-failed", evt);
    }

    public async Task PublishColumnCardsDeletedAsync(ColumnDeletionCompletedEvent evt)
    {
        await PublishSendAsync("column-cards-deleted", evt);
    }

    public async Task PublishColumnCardsDeleteFailedAsync(ColumnDeletionFailedEvent evt)
    {
        await PublishSendAsync("column-cards-delete-failed", evt);
    }

    public async Task PublishFilesDeletedAsync(FilesDeletedEvent evt)
    {
        await PublishSendAsync("files-card-delete", evt);
    }

    public async Task PublishCardDeletedAsync(CardDeletedEvent evt)
    {
        await PublishSendAsync("card-deleted", evt);
    }

    public async Task PublishCardUpdatedEmailSend(CardUpdatedEmailSendEvent evt)
    {
        await PublishSendAsync("card-update-email-send", evt);
    }
}