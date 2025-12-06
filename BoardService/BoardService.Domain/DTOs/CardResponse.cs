namespace BoardService.Domain.DTOs;

public class CardResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public List<CardLabelResponse> Labels { get; set; } = new();
    public List<CardChecklistResponse> Checklists { get; set; } = new();

    
    public List<Guid> AssigneeIds { get; set; } = new();
    public List<Guid> WatcherIds { get; set; } = new();

    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}