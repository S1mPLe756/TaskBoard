namespace AuthService.Application.DTOs.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}