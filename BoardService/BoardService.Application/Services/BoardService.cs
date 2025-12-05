using System.Net;
using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.Interfaces;
using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using ExceptionService;

namespace BoardService.Application.Services;

public class BoardService : IBoardService
{
    private IBoardRepository _repository;
    private IMapper _mapper;
    private readonly IOrganizationApiClient _organizationApiClient;
 

    public BoardService(IBoardRepository repository, IMapper mapper, IOrganizationApiClient organizationApiClient)
    {
        _repository = repository;
        _mapper = mapper;
        _organizationApiClient = organizationApiClient;
    }
    
    public async Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest)
    {
        if (!await _organizationApiClient.CanChangeWorkspaceAsync(workspaceId: createBoardRequest.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't create board", HttpStatusCode.Forbidden);
        }
        Board board = _mapper.Map<Board>(createBoardRequest);
        
        await _repository.AddBoardAsync(board);
        
        return _mapper.Map<BoardResponse>(board);
    }

    public async Task<BoardResponse> GetBoardAsync(Guid userId, Guid boardId)
    {
        var board = await _repository.GetBoardByIdAsync(boardId);

        if (board == null)
        {
            throw new AppException("Board not found", HttpStatusCode.NotFound);
        }
            
        if (!await _organizationApiClient.CanChangeWorkspaceAsync(workspaceId: board.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't see board", HttpStatusCode.Forbidden);
        }
        
        return _mapper.Map<BoardResponse>(board);
    }

    public async Task<List<BoardResponse>> GetBoardsByWorkspaceAsync(Guid userId, Guid workspaceId)
    {
        if (!await _organizationApiClient.CanChangeWorkspaceAsync(workspaceId: workspaceId,
                userId: userId))
        {
            throw new AppException("Can't see boards", HttpStatusCode.Forbidden);
        }
        
        var boards = await _repository.GetBoardsByWorkspaceAsync(workspaceId);
        
        return boards.Select(board => _mapper.Map<BoardResponse>(board)).ToList();
    }

    public async Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId)
    {
        var board = await _repository.GetBoardByIdAsync(boardId);
        if (board == null)
        {
            return false;
        }
        
        var column = board.Columns.FirstOrDefault(column => column.Id == columnId);
        
        return column != null;
    }
}