using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class DeleteCardsRequest
{
    [Required(ErrorMessage = "Board Id is required")]
    public Guid BoardId { get; set; }
    
    [Required(ErrorMessage ="Card IDs is required")]
    public List<Guid> Cards { get; set; }
}