using CardService.Domain.DTOs;
using CardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.Extensions.Logging;
using Refit;

namespace CardService.Infrastructure.ExternalAPI.Clients;

public class BoardApiClient : IBoardApiClient
{
    private readonly IBoardApiRefitClient _refitClient;
    private readonly ILogger<BoardApiClient> _logger;

    public BoardApiClient(
        IBoardApiRefitClient refitClient, 
        ILogger<BoardApiClient> logger)
    {
        _refitClient = refitClient;
        _logger = logger;
    }
    
    public async Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId)
    {
        try
        {
            _logger.LogInformation("Checking board exist {boardId} with column {columnId}", 
                boardId, columnId);
            
            return await _refitClient.IsExistBoardWithColumn(boardId, columnId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Board API for board {boardId}, column {columnId}", 
                boardId, columnId);
            throw new AppException("Board service unavailable");
        }
        
    }

    public async Task<BoardDto?> GetBoardAsync(Guid boardId)
    {
        try
        {
            _logger.LogInformation("Get board {boardId}", 
                boardId);
            
            return await _refitClient.GetBoardAsync(boardId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Board API for board {boardId}", 
                boardId);
            throw new AppException("Board service unavailable");
        }

    }
}