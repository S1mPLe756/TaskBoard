using System.ComponentModel.DataAnnotations;
using UserProfile.Application.Attributes;

namespace UserProfile.Application.DTOs.Requestes;

public class UserPreferencesUpdateDto
{
    [Required(ErrorMessage = "UserId обязателен")]
    [GuidNotEmpty(ErrorMessage = "UserId не может быть пустым GUID")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Язык обязателен")]
    [MinLength(2, ErrorMessage = "Язык должен быть минимум 2 символа (например: 'en', 'ru')")]
    public string Language { get; set; } = null!;
    
    public bool NotificationsEnabled { get; set; }
}