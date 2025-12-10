using System.ComponentModel.DataAnnotations;

namespace BoardService.Application.DTOs.Requestes;

public class CreatedCardRequest
{
    [Required(ErrorMessage = "Card Id is required")]
    public Guid CardId { get; set; }
    [Required(ErrorMessage = "Column Id is required")]
    public  Guid ColumnId { get; set; }
}