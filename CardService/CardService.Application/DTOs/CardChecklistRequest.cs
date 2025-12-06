using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs;

public class CardChecklistRequest
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Items is required")]
    public List<CardChecklistItemRequest> Items { get; set; } = new();

}