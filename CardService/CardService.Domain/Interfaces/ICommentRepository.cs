using CardService.Domain.Entities;

namespace CardService.Domain.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetCommentByIdAsync(Guid id);
    Task CreateCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task<List<Comment>> GetCommentsCardIdAsync(Guid cardId);
    Task DeleteCommentsAsync(List<Comment> comments);
    Task DeleteCommentAsync(Comment comment);
}