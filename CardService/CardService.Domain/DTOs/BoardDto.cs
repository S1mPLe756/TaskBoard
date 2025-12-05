namespace CardService.Domain.DTOs;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid WorkspaceId { get; set; }
    public List<ColumnDto> Columns { get; set; } = new();
}