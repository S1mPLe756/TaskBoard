namespace BoardService.Domain.Entities;

public class CardPosition
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CardId { get; set; }
    public Guid ColumnId { get; set; }

    public int Position { get; set; }

    public BoardColumn Column { get; set; } = null!;

}