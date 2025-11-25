namespace BoardService.Application.DTOs;

public class ColumnCardResponse
{
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
    public int Position { get; set; }
}