using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs;

public class CardChecklistItemRequest
{
    [Required(ErrorMessage = "Test is required")]
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
}