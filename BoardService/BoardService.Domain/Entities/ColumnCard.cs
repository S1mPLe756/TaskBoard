namespace BoardService.Domain.ValueObjects;

public class ColumnCard
{
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
    public int Position { get; set; }
}