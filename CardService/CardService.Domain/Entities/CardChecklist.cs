namespace CardService.Domain.Entities;

public class CardChecklist
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<CardChecklistItem> Items { get; set; } = new();

    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}
