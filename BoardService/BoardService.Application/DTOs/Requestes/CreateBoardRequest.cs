using System.ComponentModel.DataAnnotations;

namespace BoardService.Application.DTOs.Requestes;

public class CreateBoardRequest
{
    [Required(ErrorMessage = "Workspace Id is required")]
    public Guid WorkspaceId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; }
}