using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Infrastructure.Messaging.Events;
using NotificationService.Infrastructure.Settings;
using NotificationService.Infrastructure.Utils;

namespace NotificationService.Infrastructure.Messaging.Consumers;

public class NotificationCardUpdateSendConsumer: BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;
    private readonly KafkaSettings _kafkaSettings;
    

    public NotificationCardUpdateSendConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> options)
    {
        _scopeFactory = scopeFactory;

        _kafkaSettings = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = _kafkaSettings.AutoOffsetReset == "Earliest"
                ? AutoOffsetReset.Earliest
                : AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        
        _topic = _kafkaSettings.CardUpdateSend;

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
            var message = JsonSerializer.Deserialize<CardUpdatedEvent>(cr.Message.Value);

            if (message != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
                
                var emails = message.Emails
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .ToList();

                if (!emails.Any())
                    continue;

                var dto = new NotificationBulkRequest()
                {
                    Emails = emails,
                    Title = "Изменение в задаче",
                    Message = message.Message,
                    Type = NotificationType.Email
                };

                await service.SendAsync(dto, stoppingToken);
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