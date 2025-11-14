using System.Text.Json;
using Confluent.Kafka;
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

public class NotificationEmailSendConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;

    public NotificationEmailSendConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> options)
    {
        _scopeFactory = scopeFactory;

        var kafkaConfig = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaConfig.BootstrapServers,
            GroupId = kafkaConfig.GroupId,
            AutoOffsetReset = kafkaConfig.AutoOffsetReset == "Earliest"
                ? AutoOffsetReset.Earliest
                : AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        
        _topic = kafkaConfig.Topic;

        _consumer.Subscribe(_topic);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cr = _consumer.Consume(stoppingToken);
            var message = JsonSerializer.Deserialize<InvitationSendEvent>(cr.Message.Value);

            if (message != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
                
                var dto = new NotificationRequest
                {
                    To = message.Email,
                    Title = "Приглашение TaskBoard",
                    Message = EmailTemplates.Invite(message.OrganizationName),
                    Type = NotificationType.Email
                };
                
                await service.SendAsync(dto);
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