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
        if (!await _organizationApiClient.CanCreateBoardAsync(workspaceId: createBoardRequest.WorkspaceId,
                userId: userId))
        {
            throw new AppException("Can't create board", HttpStatusCode.Forbidden);
        }
        Board board = _mapper.Map<Board>(createBoardRequest);
        
        await _repository.AddBoardAsync(board);
        
        return _mapper.Map<BoardResponse>(board);
    }
}