using BoardService.Domain.DTOs;

namespace BoardService.Application.DTOs.Responses;

public class ColumnFullResponse
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }

    public List<CardResponse> Cards { get; set; } = new();
}