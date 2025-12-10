namespace CardService.Domain.DTOs;

public class CardPositionDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid ColumnId { get; set; }
}