using System.Net;
using AutoMapper;
using ExceptionService;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Responses;
using Organization.Application.Interfaces;
using Organization.Domain.Entities;
using Organization.Domain.Enums;
using Organization.Domain.Interfaces;

namespace Organization.Application.Services;

public class WorkspaceService : IWorkspaceService
{
    private IWorkspaceRepository _repository;
    private IMapper _mapper;

    public WorkspaceService(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceResponse> CreateWorkspaceAsync(Guid ownerId, string name)
    {
        var ws = new Workspace { Id = Guid.NewGuid(), Name = name, OwnerId = ownerId };
        await _repository.AddAsync(ws);
        return _mapper.Map<WorkspaceResponse>(ws);
    }

    public async Task<WorkspaceResponse> GetWorkspaceAsync(Guid workspaceId)
    {
        var workspace = await _repository.GetByIdAsync(workspaceId);
        if (workspace == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }
        
        return _mapper.Map<WorkspaceResponse>(workspace);
    }

    public async Task<List<WorkspaceResponse>> GetUserWorkspacesAsync(Guid userId)
    {
        var workspaces = await _repository.GetByUserIdAsync(userId);
        
        return workspaces.Select(_mapper.Map<WorkspaceResponse>).ToList();
    }

    public async Task<WorkspaceMemberResponse> AddMemberAsync(Guid workspaceId, Guid userId, WorkspaceRole role)
    {
        var ws = await _repository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);

        if (ws.Members.Any(m => m.UserId == userId)) throw new AppException("User already a member", HttpStatusCode.Conflict);

        var member = new WorkspaceMember { UserId = userId, Role = role };
        ws.Members.Add(member);
        await _repository.UpdateAsync(ws);

        return _mapper.Map<WorkspaceMemberResponse>(member);
    }

    public async Task RemoveMemberAsync(Guid workspaceId, Guid userId)
    {
        var ws = await _repository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);

        ws.Members.RemoveAll(m => m.UserId == userId);
        await _repository.UpdateAsync(ws);
    }
}