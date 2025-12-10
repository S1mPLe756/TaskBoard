using BoardService.Domain.Entities;

namespace BoardService.Application.DTOs.Responses;

public class BoardsResponse
{
    public bool CanChangeWorkspace { get; set; }
    public List<BoardResponse> Boards { get; set; }
}