namespace UserProfile.Application.DTOs.Requestes;

public class UserPreferencesUpdateDto
{

    public Guid UserId { get; set; }
    public string Language { get; set; } = null!;
    public bool NotificationsEnabled { get; set; }
}