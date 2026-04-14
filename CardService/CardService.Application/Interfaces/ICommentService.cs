using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;

namespace CardService.Application.Interfaces;

public interface ICommentService
{
    Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, Guid userId);
    Task<List<CommentResponse>> GetCommentsByCardAsync(Guid cardId, Guid userId);
    Task DeleteComment(Guid commentId, Guid userId);
}