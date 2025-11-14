namespace NotificationService.Application.DTOs;

public class SendEmailRequest
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}