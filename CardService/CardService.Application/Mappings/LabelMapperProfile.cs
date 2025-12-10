using AutoMapper;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class LabelMapperProfile : Profile
{
    public LabelMapperProfile()
    {
        CreateMap<CardLabelRequest, CardLabel>();

        CreateMap<CardLabel, CardLabelResponse>();
    }
    
}