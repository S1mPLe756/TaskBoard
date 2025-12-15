using System.Text.Json.Serialization;
using CardService.Domain.DTOs;
using CardService.Domain.Enum;

namespace CardService.Application.DTOs.Responses;

public class CardFullResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public List<CardLabelResponse> Labels { get; set; } = new();
    public CardChecklistResponse Checklist { get; set; } = new();
    
    public List<UserResponse> Assignees { get; set; } = new();
    public List<UserResponse> Watchers { get; set; } = new();
    
    public List<CardAttachmentResponse> Attachments { get; set; } = new();


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardPriority Priority { get; set; } = CardPriority.Normal;
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}