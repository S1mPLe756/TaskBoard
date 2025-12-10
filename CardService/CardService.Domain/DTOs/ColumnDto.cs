namespace CardService.Domain.DTOs;

public class ColumnDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<CardPositionDto> Cards { get; set; } = new();
}