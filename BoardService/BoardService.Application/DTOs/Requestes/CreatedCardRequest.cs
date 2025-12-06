namespace BoardService.Application.DTOs.Requestes;

public class CreatedCardRequest
{
    public Guid CardId { get; set; }
    public  Guid ColumnId { get; set; }
}