using AutoMapper;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Requestes;
using Organization.Application.DTOs.Responses;
using Organization.Domain.Entities;

namespace Organization.Application.Mappings;

public class WorkspaceMapperProfile : Profile
{
    public WorkspaceMapperProfile()
    {
        CreateMap<CreateWorkspaceRequest, Workspace>();
        
        CreateMap<WorkspaceResponse, Workspace>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));
        CreateMap<WorkspaceMemberResponse, WorkspaceMember>();
        CreateMap<Workspace, WorkspaceResponse>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

        CreateMap<WorkspaceMember, WorkspaceMemberResponse>();

    }
}