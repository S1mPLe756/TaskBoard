namespace CardService.Application.DTOs.Responses;

public class CommentResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;

    public CommentResponse? AnsweredComment { get; set; }
    
    public DateTime CreateAt { get; set; }
}