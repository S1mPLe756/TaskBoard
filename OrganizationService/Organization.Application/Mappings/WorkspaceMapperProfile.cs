using AutoMapper;
using Organization.Application.DTOs;
using Organization.Domain.Entities;

namespace Organization.Application.Mappings;

public class WorkspaceMapperProfile : Profile
{
    public WorkspaceMapperProfile()
    {
        CreateMap<WorkspaceDto, Workspace>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));
        CreateMap<WorkspaceMemberDto, WorkspaceMember>();
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

        CreateMap<WorkspaceMember, WorkspaceMemberDto>();

    }
}