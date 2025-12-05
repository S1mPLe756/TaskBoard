namespace BoardService.Application.DTOs;

public class CreatedCardRequest
{
    public Guid CardId { get; set; }
    public  Guid ColumnId { get; set; }
}