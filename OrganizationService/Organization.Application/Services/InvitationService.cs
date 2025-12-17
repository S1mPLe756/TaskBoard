using System.Net;
using AutoMapper;
using ExceptionService;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Responses;
using Organization.Application.Interfaces;
using Organization.Domain.Entities;
using Organization.Domain.Enums;
using Organization.Domain.Events;
using Organization.Domain.Interfaces;

namespace Organization.Application.Services;

public class InvitationService(
    IInvitationRepository repository,
    IWorkspaceRepository workspaceRepository,
    IMapper mapper,
    IEventPublisher eventPublisher, IUserApiClient userApiClient)
    : IInvitationService
{
    public async Task CreateInvitationAsync(Guid workspaceId, string email, WorkspaceRole role)
    {
        var ws = await workspaceRepository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        
        var user = await userApiClient.GetUserByEmail(email);

        if (user == null)
        {
            return;
        }

        if (ws.OwnerId == user.Id)
        {
            throw new AppException("You cannot accept the same user", HttpStatusCode.Conflict);
        }
        
        var inv = new Invitation
        {
            WorkspaceId = workspaceId,
            Email = email,
            Role = role,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await repository.AddAsync(inv);
        await eventPublisher.PublishInvitationSendAsync(new InvitationSendEvent()
        {
            Email = email,
            InvitationId = inv.Id,
            OrganizationName = ws.Name
        });
    }

    public async Task<AcceptInvtitationResponse> AcceptInvitationAsync(Guid invitationId, Guid userId)
    {
        var inv = await repository.GetByIdAsync(invitationId);

        if (inv == null || inv.ExpiresAt < DateTime.UtcNow || inv.Accepted) throw new AppException("Invalid invitation");

        var user = await userApiClient.GetUserByEmail(inv.Email);

        if (user == null || user.Id != userId)
        {
            throw new AppException("Invalid user", HttpStatusCode.Forbidden);
        }
            
        var ws = await workspaceRepository.GetByIdAsync(inv.WorkspaceId);

        if (ws == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }
        
        inv.Accepted = true;
        await repository.UpdateAsync(inv);

        ws.Members.Add(new WorkspaceMember { UserId = userId, Role = inv.Role });
        await workspaceRepository.UpdateAsync(ws);

        return new()
        {
            InvitationId = inv.Id,
            WorkspaceId = ws.Id,
        };
    }

    public async Task<List<InvitationResponse>> GetPendingInvitationsAsync(string email)
    {
        var list = await repository.GetPendingByEmailAsync(email);
        return list.Select(inv => mapper.Map<InvitationResponse>(inv)).ToList();
    }

    public async Task<WorkspaceResponse> GetWorkspaceByInvitationAsync(Guid invitationId)
    {
        var inv = await repository.GetByIdAsync(invitationId);

        if (inv == null || inv.ExpiresAt < DateTime.UtcNow || inv.Accepted) throw new AppException("Invalid invitation");

        var ws = await workspaceRepository.GetByIdAsync(inv.WorkspaceId);

        if (ws == null)
        {
            throw new AppException("Workspace not found", HttpStatusCode.NotFound);
        }

        return mapper.Map<WorkspaceResponse>(ws);
    }
}