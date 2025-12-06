using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Domain.Entities;

namespace BoardService.Application.Mappings;

public class ColumnMapperProfile : Profile
{
    public ColumnMapperProfile()
    {
        CreateMap<CreateColumnBoardRequest, BoardColumn>();
        CreateMap<BoardColumn, BoardColumnResponse>()
            .ForMember(dest=>dest.Cards,opt=>opt.MapFrom(src=>src.Cards));

        CreateMap<BoardColumn, ColumnFullResponse>()
            .ForMember(dest=>dest.Cards,opt=>opt.MapFrom(src=>src.Cards.Select(x=>x.Card)));
    }
}