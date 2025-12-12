using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class UpdateCardChecklistRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Items is required")]
    public List<UpdateCardChecklistItemRequest> Items { get; set; } = new();
}