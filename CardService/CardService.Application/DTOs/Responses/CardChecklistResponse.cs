namespace CardService.Application.DTOs.Responses;

public class CardChecklistResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public List<CardChecklistItemResponse> Items { get; set; } = new();
}