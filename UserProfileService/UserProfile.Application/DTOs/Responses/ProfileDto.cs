namespace AuthService.Application.DTOs;

public class ProfileDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? AvatarFileId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public UserPreferencesDto Preferences { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}