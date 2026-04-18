using CardService.Domain.DTOs;

namespace CardService.Application.DTOs.Responses;

public class CommentResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;
    
    public UserResponse Author { get; set; }
    
    public Guid UserId { get; set; }

    public CommentResponse? AnsweredComment { get; set; }
    
    public DateTime CreatedAt { get; set; }
}