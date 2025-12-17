using System.Net;
using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Application.Interfaces;
using BoardService.Domain.Entities;
using BoardService.Domain.Enums;
using BoardService.Domain.Interfaces;
using ExceptionService;
using Hangfire;

namespace BoardService.Application.Services;

public class BoardService(
    IBoardRepository repository,
    IMapper mapper,
    IOrganizationApiClient organizationApiClient,
    ICardApiClient cardApiClient, IEventPublisher eventPublisher)
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

    public async Task<BoardResponse> GetBoardByCardIdAsync(Guid userId, Guid cardId)
    {
        var board = await repository.GetBoardByCardIdAsync(cardId);

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

    public async Task<BoardsResponse> GetBoardsByWorkspaceAsync(Guid userId, Guid workspaceId)
    {
        if (!await organizationApiClient.CanSeeWorkspaceAsync(workspaceId: workspaceId,
                userId: userId))
        {
            throw new AppException("Can't see boards", HttpStatusCode.Forbidden);
        }

        var isCanChange = await organizationApiClient.CanChangeWorkspaceAsync(workspaceId, userId);
        
        var boards = await repository.GetBoardsByWorkspaceAsync(workspaceId);
        
        return new BoardsResponse()
        {
            CanChangeWorkspace = isCanChange,
            Boards = boards.Where(b=>b.Status == DeletionStatus.NotDeleted).Select(board => mapper.Map<BoardResponse>(board)).ToList()
        };
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
        board.Columns = board.Columns.OrderBy(c => c.Position).ToList();

        if (cardIds.Count != 0)
        {
            var cardsResponse = await cardApiClient.GetCards(new ()
            {
                CardIds = cardIds
            });
            
            foreach (var column in board.Columns)
            {
                column.Cards = column.Cards.OrderBy(c => c.Position).ToList();
                
                foreach (var cardRef in column.Cards)
                {
                    cardRef.Card = cardsResponse.FirstOrDefault(c => c.Id == cardRef.CardId);
                }
                
                column.Cards = column.Cards.Where(x=>x.Card != null).ToList();
            }
        }
        
        return mapper.Map<BoardFullResponse>(board);
    }

    public async Task DeleteBoardAsync(Guid userId, Guid boardId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }
            
        if (!await organizationApiClient.CanChangeWorkspaceAsync(workspaceId: board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't see board", HttpStatusCode.Forbidden);
        }

        board.Status = DeletionStatus.Deleting;
        
        await repository.UpdateBoardAsync(board);
        
        var cardIds = board.Columns.SelectMany(column => column.Cards).Select(x=>x.CardId).ToList();

        try
        {
            await eventPublisher.PublishBoardDeleteSendAsync(new()
            {
                BoardId = boardId,
                CardIds = cardIds,
            });
        }
        catch (Exception ex)
        {
            board.Status = DeletionStatus.NotDeleted;
            await repository.UpdateBoardAsync(board);
            throw new AppException(("Failed to publish board deletion event: " + ex.Message), HttpStatusCode.InternalServerError);
        }

    }
    

    public async Task CompleteBoardDeletionAsync(Guid evtBoardId)
    {
        var board = await repository.GetBoardByIdAsync(evtBoardId);
        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }
        
        await repository.DeleteBoardAsync(board);
    }

    public async Task FailedBoardDeleteAsync(Guid boardId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            return;
        }

        board.Status = DeletionStatus.DeletionFailed;

        await repository.UpdateBoardAsync(board);
        
        BackgroundJob.Schedule(
            () => RetryDeleteBoard(boardId),
            TimeSpan.FromMinutes(5)
        );

    }

    public async Task UpdateBoardAsync(Guid userId, Guid boardId, UpdateBoardRequest request)
    {
        var board = await repository.GetBoardByIdAsync(boardId);
        
        
        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }
            
        if (!await organizationApiClient.CanChangeWorkspaceAsync(workspaceId: board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't change board", HttpStatusCode.Forbidden);
        }
        
        board.Title = request.Title;
        
        await repository.UpdateBoardAsync(board);
    }

    private async Task RetryDeleteBoard(Guid boardId)
    {
        var board = await repository.GetBoardByIdAsync(boardId);
        if (board == null || board.Status != DeletionStatus.DeletionFailed) return;

        board.Status = DeletionStatus.Deleting;
        await repository.UpdateBoardAsync(board);

        var cardIds = board.Columns.SelectMany(c => c.Cards).Select(c => c.CardId).ToList();

        await eventPublisher.PublishBoardDeleteSendAsync(new()
        {
            BoardId = boardId,
            CardIds = cardIds,
        });
    }
}