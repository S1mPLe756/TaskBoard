using System.Text.Json;
using BoardService.Application.DTOs;
using BoardService.Application.Interfaces;
using BoardService.Infrastructure.Messaging.Events;
using BoardService.Infrastructure.Settings;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BoardService.Infrastructure.Messaging.Consumers;

public class CardCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;
    private readonly KafkaSettings _kafkaSettings;

    public CardCreatedConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> options)
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
        
        _topic = _kafkaSettings.Topic;

        

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _kafkaSettings.BootstrapServers }).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[] { 
                    new() { Name = _topic, ReplicationFactor = 1, NumPartitions = 1 } });
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
            var message = JsonSerializer.Deserialize<CardCreatedEvent>(cr.Message.Value);

            if (message != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICardService>();

                var cardDto = new CreatedCardRequest()
                {
                    CardId = message.CardId,
                    ColumnId = message.ColumnId,
                };
                await service.CreateCardPositionAsync(cardDto);
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