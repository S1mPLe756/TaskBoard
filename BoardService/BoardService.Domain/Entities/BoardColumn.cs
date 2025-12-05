namespace BoardService.Domain.Entities;

public class BoardColumn
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Position { get; set; }

    public Guid BoardId { get; set; }
    public Board Board { get; set; }

    public List<CardPosition> Cards { get; set; } = new();

}