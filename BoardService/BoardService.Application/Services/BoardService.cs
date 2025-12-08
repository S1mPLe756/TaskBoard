using System.Net;
using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Application.Interfaces;
using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using ExceptionService;

namespace BoardService.Application.Services;

public class BoardService(
    IBoardRepository repository,
    IMapper mapper,
    IOrganizationApiClient organizationApiClient,
    ICardApiClient cardApiClient)
    : IBoardService
{

    public async Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest)
    {
        if (!await organizationApiClient.CanChangeWorkspaceAsync(workspaceId: createBoardRequest.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't create board", HttpStatusCode.Forbidden);
        }
        Board board = mapper.Map<Board>(createBoardRequest);
        
        await repository.AddBoardAsync(board);
        
        return mapper.Map<BoardResponse>(board);
    }

    public async Task<BoardResponse> GetBoardAsync(Guid userId, Guid boardId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }
            
        if (!await organizationApiClient.CanSeeWorkspaceAsync(workspaceId: board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't see board", HttpStatusCode.Forbidden);
        }
        
        return mapper.Map<BoardResponse>(board);
    }

    public async Task<List<BoardResponse>> GetBoardsByWorkspaceAsync(Guid userId, Guid workspaceId)
    {
        if (!await organizationApiClient.CanSeeWorkspaceAsync(workspaceId: workspaceId,
                userId: userId))
        {
            throw new AppException("Can't see boards", HttpStatusCode.Forbidden);
        }
        
        var boards = await repository.GetBoardsByWorkspaceAsync(workspaceId);
        
        return boards.Select(board => mapper.Map<BoardResponse>(board)).ToList();
    }

    public async Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);
        if (board == null)
        {
            return false;
        }
        
        var column = board.Columns.FirstOrDefault(column => column.Id == columnId);
        
        return column != null;
    }

    public async Task<BoardFullResponse> GetBoardFullAsync(Guid userId, Guid boardId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }

        if (!await organizationApiClient.CanSeeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("Can't see board", HttpStatusCode.Forbidden);
        }

        var cardIds = board.Columns.SelectMany(column => column.Cards).Select(x=>x.CardId).ToList();

        if (cardIds.Count != 0)
        {
            var cardsResponse = await cardApiClient.GetCards(new ()
            {
                CardIds = cardIds
            });
            
            foreach (var column in board.Columns)
            {
                foreach (var cardRef in column.Cards)
                {
                    cardRef.Card = cardsResponse.FirstOrDefault(c => c.Id == cardRef.CardId);
                }
            }
        }
        
        return mapper.Map<BoardFullResponse>(board);
    }
}