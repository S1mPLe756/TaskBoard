namespace CardService.Domain.Entities;

public class CardChecklistItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public Guid ChecklistId { get; set; }
    public CardChecklist Checklist { get; set; } = null!;
}
