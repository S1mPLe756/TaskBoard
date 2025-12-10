using System.Text.Json.Serialization;
using CardService.Domain.Enum;

namespace CardService.Application.DTOs.Responses;

public class CardResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public List<CardLabelResponse> Labels { get; set; } = new();
    public List<CardChecklistResponse> Checklists { get; set; } = new();
    
    public List<Guid> AssigneeIds { get; set; } = new();
    public List<Guid> WatcherIds { get; set; } = new();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardPriority Priority { get; set; } = CardPriority.Normal;
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}