using System.ComponentModel.DataAnnotations;

namespace CardService.Application.DTOs.Requests;

public class UpdateCardChecklistItemRequest
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Текст обязателен")]
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    
    public int Position { get; set; } = 0;
}