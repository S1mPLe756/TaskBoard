using System.Net;
using AutoMapper;
using ExceptionService;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Requestes;
using Organization.Application.DTOs.Responses;
using Organization.Application.Interfaces;
using Organization.Domain.Entities;
using Organization.Domain.Enums;
using Organization.Domain.Interfaces;

namespace Organization.Application.Services;

public class WorkspaceService(IWorkspaceRepository repository, IMapper mapper, IUserApiClient userApi) : IWorkspaceService
{
    public async Task<WorkspaceResponse> CreateWorkspaceAsync(Guid ownerId, CreateWorkspaceRequest request)
    {
        var ws = mapper.Map<Workspace>(request);
        
        ws.OwnerId = ownerId;
        
        await repository.AddAsync(ws);
        return mapper.Map<WorkspaceResponse>(ws);
    }

    
    public async Task<bool> CanChangeWorkspaceAsync(Guid userId, Guid workspaceId)
    {
        var ws = await repository.GetByIdAsync(workspaceId);
        
        if (ws == null) return false;
        
        return ws.OwnerId == userId || ws.Members.Any(x => x.UserId == userId && x.Role == WorkspaceRole.Admin);
    }

    public async Task<bool> CanSeeWorkspace(Guid userId, Guid workspaceId)
    {
        var ws = await repository.GetByIdAsync(workspaceId);
        
        if (ws == null) return false;
        
        return ws.OwnerId == userId || ws.Members.Any(x => x.UserId == userId);
    }

    public async Task<MemberResponse> GetMembersAsync(Guid workspaceId, Guid userId)
    {
        var ws = await repository.GetByIdAsync(workspaceId);

        if (ws == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }
        
        if (ws.OwnerId != userId && ws.Members.All(x => x.UserId != userId))
        {
            throw new AppException("Can't see workspace members", HttpStatusCode.Forbidden);
        }

        var members = ws.Members.Where(x => x.UserId != userId).Select(x => mapper.Map<MemberUserResponse>(x)).ToList();
        var ids = members.Select(x => x.UserId).ToList();
        if (ids.Count > 0)
        {
            var users = await userApi.GetUsers(ids);

            foreach (var member in members)
            {
                var user = users.FirstOrDefault(u => u.Id == member.UserId);
                member.User = user;
            }
        }
        
        
        return new()
        {
            IsCanChange = ws.OwnerId == userId || ws.Members.Any(x => x.Id == userId && x.Role == WorkspaceRole.Admin),
            Members = members
        };
    }

    public async Task DeleteWorkspaceAsync(Guid workspaceId, Guid userId)
    {
        var workspace = await repository.GetByIdAsync(workspaceId);

        if (workspace == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }
        
        if (workspace.OwnerId == userId)
        {
            await repository.DeleteAsync(workspace);
        }
        else if (workspace.Members.Any(x => x.UserId == userId))
        {
            workspace.Members.RemoveAll(x => x.UserId == userId);
            await repository.UpdateAsync(workspace);
        }
        else
        {
            throw new AppException("You can't delete workspace", HttpStatusCode.Forbidden);
        }
    }

    public async Task ChangeWorkspaceAsync(ChangeWorkspaceRequest request, Guid workspaceId, Guid userId)
    {
        var workspace = await repository.GetByIdAsync(workspaceId);

        if (workspace == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }

        if (workspace.OwnerId != userId)
        {
            throw new AppException("Can't change workspace", HttpStatusCode.Forbidden);
        }
        
        workspace.Name = request.Name;
        workspace.Description = request.Description;
        await repository.UpdateAsync(workspace);
    }

    public async Task<WorkspaceResponse> GetWorkspaceAsync(Guid workspaceId)
    {
        var workspace = await repository.GetByIdAsync(workspaceId);
        if (workspace == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }
        
        return mapper.Map<WorkspaceResponse>(workspace);
    }

    public async Task<List<WorkspaceResponse>> GetUserWorkspacesAsync(Guid userId)
    {
        var workspaces = await repository.GetByUserIdAsync(userId);
        
        return workspaces.Select(workspace =>
        {
            var dto = mapper.Map<WorkspaceResponse>(workspace);
            dto.IsYour = workspace.OwnerId == userId;
            return dto;
        }).ToList();
    }

    public async Task<WorkspaceMemberResponse> AddMemberAsync(Guid workspaceId, Guid userId, WorkspaceRole role)
    {
        var ws = await repository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);

        if (ws.Members.Any(m => m.UserId == userId)) throw new AppException("User already a member", HttpStatusCode.Conflict);

        var member = new WorkspaceMember { UserId = userId, Role = role };
        ws.Members.Add(member);
        await repository.UpdateAsync(ws);

        return mapper.Map<WorkspaceMemberResponse>(member);
    }

    public async Task RemoveMemberAsync(Guid workspaceId, Guid userId)
    {
        var ws = await repository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);

        ws.Members.RemoveAll(m => m.UserId == userId);
        await repository.UpdateAsync(ws);
    }
}