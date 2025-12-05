namespace CardService.Domain.Entities;

public class CardLabel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#00BFFF";

    public List<Card> Cards { get; set; } = new();
}
