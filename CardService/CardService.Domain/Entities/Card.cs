using CardService.Domain.Enum;

namespace CardService.Domain.Entities;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<CardLabel> Labels { get; set; } = new();
    public List<CardChecklist> Checklists { get; set; } = new();

    public List<Guid> AssigneeIds { get; set; } = new();
    public List<Guid> WatcherIds { get; set; } = new();

    public CardPriority Priority { get; set; } = CardPriority.Normal;
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}