namespace BoardService.Application.DTOs;

public class BoardColumnResponse
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }

    public List<ColumnCardResponse> Cards { get; set; } = new();

}