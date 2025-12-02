using System.ComponentModel.DataAnnotations;
using UserProfile.Application.Attributes;

namespace UserProfile.Application.DTOs.Requestes;

public class UserProfileDto
{
    [Required(ErrorMessage = "UserId обязателен")]
    [GuidNotEmpty(ErrorMessage = "UserId не может быть пустым GUID")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [MaxLength(150, ErrorMessage = "Email не может превышать 150 символов")]
    public string Email { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Имя не может превышать 100 символов")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Некорректный номер телефона")]
    [MaxLength(30, ErrorMessage = "Телефон не может превышать 30 символов")]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Bio не может превышать 500 символов")]
    public string Bio { get; set; } = string.Empty;
}