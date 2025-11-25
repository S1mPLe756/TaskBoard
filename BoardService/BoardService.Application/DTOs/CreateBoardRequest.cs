namespace BoardService.Application.DTOs;

public class CreateBoardRequest
{
    public Guid WorkspaceId { get; set; }
    public string Title { get; set; }
}