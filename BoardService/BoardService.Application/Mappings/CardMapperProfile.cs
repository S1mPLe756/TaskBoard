using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Domain.Entities;

namespace BoardService.Application.Mappings;

public class CardMapperProfile : Profile
{

    public CardMapperProfile()
    {
        CreateMap<CreatedCardRequest, CardPosition>();
        
        CreateMap<CardPosition, ColumnCardResponse>();

    }
    
}