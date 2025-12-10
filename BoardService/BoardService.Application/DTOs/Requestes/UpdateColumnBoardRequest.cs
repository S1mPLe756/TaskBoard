using System.ComponentModel.DataAnnotations;

namespace BoardService.Application.DTOs.Requestes;

public class UpdateColumnBoardRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Position is required")]
    public int Position { get; set; }
}