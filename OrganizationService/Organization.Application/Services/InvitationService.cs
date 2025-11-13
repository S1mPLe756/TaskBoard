using System.Net;
using AutoMapper;
using ExceptionService;
using Organization.Application.DTOs;
using Organization.Application.Interfaces;
using Organization.Domain.Entities;
using Organization.Domain.Enums;
using Organization.Domain.Interfaces;

namespace Organization.Application.Services;

public class InvitationService : IInvitationService
{
    private IInvitationRepository _repository;
    private IWorkspaceRepository _workspaceRepository;
    private IMapper _mapper;

    public InvitationService(IInvitationRepository repository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _repository = repository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    
    public async Task<InvitationDto> CreateInvitationAsync(Guid workspaceId, string email, WorkspaceRole role)
    {
        var ws = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (ws == null) throw new AppException("Workspace not found", HttpStatusCode.NotFound);

        var inv = new Invitation
        {
            WorkspaceId = workspaceId,
            Email = email,
            Role = role,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await _repository.AddAsync(inv);

        return _mapper.Map<InvitationDto>(inv);
    }

    public async Task AcceptInvitationAsync(Guid invitationId, Guid userId)
    {
        var inv = await _repository.GetByIdAsync(invitationId);
        if (inv == null || inv.ExpiresAt < DateTime.UtcNow) throw new AppException("Invalid invitation");

        inv.Accepted = true;
        await _repository.UpdateAsync(inv);

        var ws = await _workspaceRepository.GetByIdAsync(inv.WorkspaceId);
        ws.Members.Add(new WorkspaceMember { UserId = userId, Role = inv.Role });
        await _workspaceRepository.UpdateAsync(ws);
    }

    public async Task<List<InvitationDto>> GetPendingInvitationsAsync(string email)
    {
        var list = await _repository.GetPendingByEmailAsync(email);
        return list.Select(inv => _mapper.Map<InvitationDto>(inv)).ToList();
    }
}