namespace CardService.Domain.Entities;

public class CardChecklistItem
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public int Position { get; set; } = 0;
    public Guid ChecklistId { get; set; }
    public CardChecklist Checklist { get; set; } = null!;
}
