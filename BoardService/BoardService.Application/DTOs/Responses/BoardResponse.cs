namespace BoardService.Application.DTOs.Responses;

public class BoardResponse
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Title { get; set; }
    public bool IsArchived { get; set; }

    public List<BoardColumnResponse> Columns { get; set; } = new();

}