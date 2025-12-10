using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class DeleteColumnCardsRequest
{
    [Required(ErrorMessage = "Column Id is required")]
    public Guid ColumnId { get; set; }
    
    [Required(ErrorMessage ="Card IDs is required")]
    public List<Guid> Cards { get; set; }
}