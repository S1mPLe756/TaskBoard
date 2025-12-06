namespace BoardService.Domain.DTOs;

public class CardChecklistItemResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}