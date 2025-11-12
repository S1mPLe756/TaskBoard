namespace UserProfile.Domain.Entities;

public class UserPreferences
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public Profile Profile { get; set; } = default!;

    public string Language { get; set; } = "en";
    public bool NotificationsEnabled { get; set; } = true;
}