using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class CardLabelRequest
{
    [Required(ErrorMessage = "Title is required")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Color is required")]
    public string Color { get; set; } = "#00BFFF";
}