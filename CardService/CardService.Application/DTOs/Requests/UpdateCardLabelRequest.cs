using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class UpdateCardLabelRequest
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Название обязательно")]
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = "#00BFFF";

}