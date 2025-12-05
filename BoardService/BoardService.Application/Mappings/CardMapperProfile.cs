using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Domain.Entities;

namespace BoardService.Application.Mappings;

public class CardMapperProfile : Profile
{

    public CardMapperProfile()
    {
        CreateMap<CreatedCardRequest, CardPosition>();
    }
    
}