using AutoMapper;
using Organization.Application.DTOs;
using Organization.Domain.Entities;

namespace Organization.Application.Mappings;

public class InvitationMapperProfile : Profile
{
    public InvitationMapperProfile()
    {
        CreateMap<InvitationResponse, Invitation>();
        CreateMap<Invitation, InvitationResponse>();

    }
}