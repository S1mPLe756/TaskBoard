using AutoMapper;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class CardMapperProfile : Profile
{
    public CardMapperProfile()
    {
        CreateMap<CreateCardRequest, Card>();
        CreateMap<Card, CardResponse>();
        CreateMap<Card, CardFullResponse>();

        
    }
}