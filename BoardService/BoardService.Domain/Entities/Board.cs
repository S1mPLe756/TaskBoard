using BoardService.Domain.Enums;

namespace BoardService.Domain.Entities;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public Guid WorkspaceId { get; set; }
    public bool IsArchived { get; set; }
    
    public DeletionStatus Status { get; set; } = DeletionStatus.NotDeleted;

    public List<BoardColumn> Columns { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}