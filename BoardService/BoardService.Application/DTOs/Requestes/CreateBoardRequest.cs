namespace BoardService.Application.DTOs.Requestes;

public class CreateBoardRequest
{
    public Guid WorkspaceId { get; set; }
    public string Title { get; set; }
}