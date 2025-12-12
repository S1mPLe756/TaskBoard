namespace BoardService.Domain.DTOs;

public class CardResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    
    public List<CardLabelResponse> Labels { get; set; } = new();

    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
}