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

        await CheckAsync(
            request.CardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );


        Comment comment = mapper.Map<Comment>(request);
        
        comment.UserId = userId;
        
        await repository.CreateCommentAsync(comment);
        
        return mapper.Map<CommentResponse>(comment);
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
        
        return (await repository.GetCommentsCardIdAsync(cardId)).Select(comment => mapper.Map<CommentResponse>(comment)).ToList();
    }

    public Task DeleteComment(Guid commentId, Guid userId)
    {
        throw new NotImplementedException();
    }
}