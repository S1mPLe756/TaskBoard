using AutoMapper;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class CardChecklistMapperProfile : Profile
{
    public CardChecklistMapperProfile()
    {
        CreateMap<CardChecklistItemRequest, CardChecklistItem>();
        CreateMap<CardChecklistRequest, CardChecklist>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


        CreateMap<CardChecklistItem, CardChecklistItemResponse>();
        CreateMap<CardChecklist, CardChecklistResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}