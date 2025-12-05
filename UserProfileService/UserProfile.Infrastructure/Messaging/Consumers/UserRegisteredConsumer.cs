using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Infrastructure.Messaging.Events;
using UserProfile.Application.Interfaces;
using UserProfile.Infrastructure.Settings;

namespace UserProfile.Infrastructure.Messaging.Consumers;

public class UserRegisteredConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;
    private KafkaSettings _kafkaSettings;

    public UserRegisteredConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> options)
    {
        _scopeFactory = scopeFactory;

        var kafkaConfig = options.Value;
        _kafkaSettings = kafkaConfig;
        
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
            var message = JsonSerializer.Deserialize<UserRegisteredEvent>(cr.Message.Value);

            if (message != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IUserProfileService>();
                
                var dto = new UserProfileDto
                {
                    UserId = message.UserId,
                };
                
                await service.CreateProfileAsync(dto);
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