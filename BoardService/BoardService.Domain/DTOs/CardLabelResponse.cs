namespace BoardService.Domain.DTOs;

public class CardLabelResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#00BFFF";
}