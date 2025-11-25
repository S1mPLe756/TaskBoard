namespace BoardService.Domain.Entities;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public Guid WorkspaceId { get; set; }
    public bool IsArchived { get; set; }

    public List<BoardColumn> Columns { get; set; } = new();

}