namespace BoardService.Application.DTOs.Responses;

public class BoardFullResponse
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Title { get; set; }
    public bool IsArchived { get; set; }

    public List<ColumnFullResponse> Columns { get; set; } = new();
}