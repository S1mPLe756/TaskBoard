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

public class ColumnService(
    IColumnRepository columnRepository,
    IBoardRepository boardRepository,
    IMapper mapper,
    IOrganizationApiClient organizationApiClient, IEventPublisher eventPublisher, IBackgroundJobClient _jobClient)
    : IColumnService
{

    public async Task<BoardColumnResponse> CreateColumnForBoardAsync(CreateColumnBoardRequest request, Guid boardId, Guid userId)
    {
        var board = await boardRepository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }

        if (! await organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't create column", HttpStatusCode.Forbidden);
        }
        
        var column = mapper.Map<BoardColumn>(request);
        
        column.BoardId = boardId;
        
        await columnRepository.AddColumnAsync(column);
        
        return mapper.Map<BoardColumnResponse>(column);
    }

    public async Task<List<BoardColumnResponse>> GetColumnsBoard(Guid boardId, Guid userId)
    {
        var board = await boardRepository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }

        if (! await organizationApiClient.CanSeeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't see columns", HttpStatusCode.Forbidden);
        }
        
        var columns = await columnRepository.GetColumnByBoardIdAsync(boardId);
        
        return columns.Select(c => mapper.Map<BoardColumnResponse>(c)).ToList();
    }

    public async Task DeleteColumnAsync(Guid columnId, Guid userId)
    {
        var column = await columnRepository.GetColumnByIdAsync(columnId);

        if (column == null)
        {
            throw new AppException("Column not found", HttpStatusCode.NotFound);
        }
        
            
        if (!await organizationApiClient.CanChangeWorkspaceAsync(workspaceId: column.Board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't change board", HttpStatusCode.Forbidden);
        }

        column.Status = DeletionStatus.Deleting;
        
        await columnRepository.UpdateColumnAsync(column);
        
        var cardIds = column.Cards.Select(x=>x.CardId).ToList();

        try
        {
            await eventPublisher.PublishColumnDeleteSendAsync(new()
            {
                ColumnId = columnId,
                CardIds = cardIds,
            });
        }
        catch (Exception ex)
        {
            column.Status = DeletionStatus.NotDeleted;
            await columnRepository.UpdateColumnAsync(column);
            throw new AppException(("Failed to publish column deletion event: " + ex.Message), HttpStatusCode.InternalServerError);
        }
    }

    public async Task CompleteColumnDeletionAsync(Guid evtColumnId)
    {
        var column = await columnRepository.GetColumnByIdAsync(evtColumnId);
        
        if (column == null)
        {
            throw new AppException("Column not found", HttpStatusCode.NotFound);
        }
        
        if (column.Status != DeletionStatus.Deleting)
        {
            return;
        }
        
        await columnRepository.DeleteColumnAsync(column);
    }

    public async Task FailedColumnDeleteAsync(Guid evtColumnId)
    {
        var column = await columnRepository.GetColumnByIdAsync(evtColumnId);

        if (column == null)
        {
            return;
        }

        column.Status = DeletionStatus.DeletionFailed;

        await columnRepository.UpdateColumnAsync(column);
        
        _jobClient.Schedule(
            () => RetryDeleteColumn(evtColumnId),
            TimeSpan.FromMinutes(5)
        );
        
    }

    public async Task<BoardColumnResponse> UpdateColumnAsync(Guid columnId, Guid userId,
        UpdateColumnBoardRequest request)
    {
        var column = await columnRepository.GetColumnByIdAsync(columnId);

        if (column == null)
        {
            throw new AppException("Column not found", HttpStatusCode.NotFound);
        }

        if (!await organizationApiClient.CanChangeWorkspaceAsync(workspaceId: column.Board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't change board", HttpStatusCode.Forbidden);
        }

        column.Name = request.Name;
        column.Position = request.Position;
        
        await columnRepository.UpdateColumnAsync(column);
        return mapper.Map<BoardColumnResponse>(column);
    }


    public async Task RetryDeleteColumn(Guid evtColumnId)
    {
        var column = await columnRepository.GetColumnByIdAsync(evtColumnId);
        if (column == null || column.Status != DeletionStatus.DeletionFailed) return;

        column.Status = DeletionStatus.Deleting;
        await columnRepository.UpdateColumnAsync(column);

        var cardIds = column.Cards.Select(c => c.CardId).ToList();

        await eventPublisher.PublishColumnDeleteSendAsync(new()
        {
            ColumnId = evtColumnId,
            CardIds = cardIds,
        });
    }
}