using System.Text.Json;
using CardService.Application.DTOs.Requests;
using CardService.Application.Interfaces;
using CardService.Domain.Events;
using CardService.Infrastructure.Settings;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CardService.Infrastructure.Messaging.Consumers;

public class BoardDeleteConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;
    private readonly KafkaSettings _kafkaSettings;

    public BoardDeleteConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> options)
    {
        _scopeFactory = scopeFactory;

        _kafkaSettings = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = _kafkaSettings.AutoOffsetReset == "Earliest"
                ? AutoOffsetReset.Earliest
                : AutoOffsetReset.Latest,
            AllowAutoCreateTopics = true,
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        _topic = _kafkaSettings.DeleteBoardTopic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var adminClient = new AdminClientBuilder(new AdminClientConfig
                   { BootstrapServers = _kafkaSettings.BootstrapServers }).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[]
                {
                    new() { Name = _topic, ReplicationFactor = 1, NumPartitions = 1 }
                });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }

        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var cr = _consumer.Consume(stoppingToken);
            var message = JsonSerializer.Deserialize<BoardDeleteEvent>(cr.Message.Value);

            if (message != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICardService>();

                var deleteRequest = new DeleteCardsRequest()
                {
                    Cards = message.CardIds,
                    BoardId = message.BoardId
                };
                await service.DeleteCardsAsync(deleteRequest);
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}