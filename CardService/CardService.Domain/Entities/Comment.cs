using System.Runtime.CompilerServices;

namespace CardService.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? AnsweredCommentId { get; set; } = null;
    
    public Comment? AnsweredComment { get; set; } = null;
    
    public List<Comment> AnswerComments { get; set; } = new();

    public Guid UserId { get; set; }
    
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}