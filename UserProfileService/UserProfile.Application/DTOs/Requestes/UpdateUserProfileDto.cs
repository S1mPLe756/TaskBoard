using System.ComponentModel.DataAnnotations;

namespace UserProfile.Application.DTOs.Requestes;

public class UpdateUserProfileDto
{
    [MaxLength(100, ErrorMessage = "Имя не может превышать 100 символов")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Некорректный номер телефона")]
    [MaxLength(30, ErrorMessage = "Телефон не может превышать 30 символов")]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Bio не может превышать 500 символов")]
    public string Bio { get; set; } = string.Empty;
}