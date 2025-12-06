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

public class ColumnService : IColumnService
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;
    private readonly IOrganizationApiClient _organizationApiClient;

    public ColumnService(IColumnRepository columnRepository, IBoardRepository boardRepository, IMapper mapper, IOrganizationApiClient organizationApiClient)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
        _mapper = mapper;
        _organizationApiClient = organizationApiClient;
    }

    public async Task<BoardColumnResponse> CreateColumnForBoardAsync(CreateColumnBoardRequest request, Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }

        if (! await _organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't create column", HttpStatusCode.Forbidden);
        }
        
        var column = _mapper.Map<BoardColumn>(request);
        
        column.BoardId = boardId;
        
        await _columnRepository.AddColumnAsync(column);
        
        return _mapper.Map<BoardColumnResponse>(column);
    }

    public async Task<List<BoardColumnResponse>> GetColumnsBoard(Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }

        if (! await _organizationApiClient.CanSeeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't see columns", HttpStatusCode.Forbidden);
        }
        
        var columns = await _columnRepository.GetColumnByBoardIdAsync(boardId);
        
        return columns.Select(c => _mapper.Map<BoardColumnResponse>(c)).ToList();
    }
}