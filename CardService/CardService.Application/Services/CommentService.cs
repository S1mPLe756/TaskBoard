using System.Net;
using AutoMapper;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Application.Interfaces;
using CardService.Domain.DTOs;
using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using ExceptionService;

namespace CardService.Application.Services;

public class CommentService(
    ICommentRepository repository,
    IMapper mapper,
    IBoardApiClient boardApiClient,
    IOrganizationApiClient organizationApiClient,
    ICardRepository cardRepository,
    IUserApiClient userApiClient) : ICommentService
{
    public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, Guid userId)
    {
        var card = await cardRepository.GetCardByIdAsync(request.CardId);

        if (card == null)
        {
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        }

        Comment? answeredComment = null;
        if (request.AnsweredCommentId.HasValue)
        {
            answeredComment = await repository.GetCommentByIdAsync(request.AnsweredCommentId.Value);
            if(answeredComment == null)
                throw new AppException("Reply comment not found", HttpStatusCode.NotFound);
        }


        await CheckAsync(
            request.CardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );


        Comment comment = mapper.Map<Comment>(request);
        
        comment.UserId = userId;
        
        await repository.CreateCommentAsync(comment);
        
        comment.AnsweredComment = answeredComment;

        var response = mapper.Map<CommentResponse>(comment);

        await fillAuthors(userId, response);

        return response;
    }

    private async Task fillAuthors(Guid userId, CommentResponse response)
    {
        var usersId = new List<Guid>() { userId };

        if (response.AnsweredComment != null)
        {
            usersId.Add(response.AnsweredComment.UserId);
        }

        var users = await userApiClient.GetUsers(usersId);
        
        response.Author = users.First(x => x.Id == response.UserId);

        if (response.AnsweredComment != null)
        {
            response.AnsweredComment.Author = users.First(x => x.Id == response.AnsweredComment.UserId);
        }
    }

    private async Task CheckAsync(
        Guid id,
        Guid userId,
        Func<Guid, Guid, Task<bool>> permissionCheck,
        Func<Guid, Task<BoardDto?>> boardResolver)
    {
        var board = await boardResolver(id)
                    ?? throw new AppException("Board not found", HttpStatusCode.NotFound);

        if (!await permissionCheck(board.WorkspaceId, userId))
            throw new AppException("Access denied", HttpStatusCode.Forbidden);
    }

    public async Task<List<CommentResponse>> GetCommentsByCardAsync(Guid cardId, Guid userId)
    {
        await CheckAsync(
            cardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );

        var comments = await repository.GetCommentsCardIdAsync(cardId);

        var commentsResponses = comments.Select(comment => mapper.Map<CommentResponse>(comment)).ToList();
        await FillUsersAsync(comments, commentsResponses);
        
        return commentsResponses;
    }
    
    private async Task FillUsersAsync(List<Comment> comments, List<CommentResponse> commentsResponses)
    {
        var usersId = comments.Select(comment => comment.UserId).ToList();
        
        var users = await userApiClient.GetUsers(usersId);
        
        commentsResponses.ForEach(comment => comment.Author = users.First(user => user.Id == comment.UserId));
    }


    public async Task DeleteComment(Guid commentId, Guid userId)
    {
        var comment = await repository.GetCommentByIdAsync(commentId);
        
        await CheckAsync(
            comment.CardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );

        if (comment.UserId != userId)
        {
            throw new AppException("Нет прав на удаление комментария", HttpStatusCode.Forbidden);
        }
        
        await repository.DeleteCommentAsync(comment);
    }
}