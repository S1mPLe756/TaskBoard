namespace BoardService.Infrastructure.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "";
    public string GroupId { get; set; } = "";
    public string AutoOffsetReset { get; set; } = "Earliest";
    public string CardCreatedTopic { get; set; } = "";
    public string DeletedTopic { get; set; } = "";
    public string DeleteFailedTopic { get; set; } = "";
    public string DeletedColumnTopic { get; set; } = "";
    public string DeleteColumnFailedTopic { get; set; } = "";
}