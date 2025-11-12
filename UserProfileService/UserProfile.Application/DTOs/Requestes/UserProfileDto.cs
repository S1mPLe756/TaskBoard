namespace UserProfile.Application.DTOs.Requestes;

public class UserProfileDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!; 
}