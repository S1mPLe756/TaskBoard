namespace AuthService.Application.DTOs;

public class UserPreferencesDto
{
    public string Language { get; set; } = "en";
    public bool NotificationsEnabled { get; set; } = true;
}