using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Почта обязательна")]
    [EmailAddress(ErrorMessage = "Не верный формат почты")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Пароль должен иметь от 2 до 20 символов")]
    public string Password { get; set; } = null!;
}