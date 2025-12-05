using AutoMapper;
using CardService.Application.DTOs;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class CardMapperProfile : Profile
{
    public CardMapperProfile()
    {
        CreateMap<CreateCardRequest, Card>();
        CreateMap<Card, CardResponse>();
    }
}