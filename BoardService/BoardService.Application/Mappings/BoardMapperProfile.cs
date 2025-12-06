using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Domain.Entities;

namespace BoardService.Application.Mappings;

public class BoardMapperProfile : Profile
{
    public BoardMapperProfile()
    {
        CreateMap<CreateBoardRequest, Board>();
        CreateMap<Board, BoardResponse>();
        CreateMap<BoardColumn, BoardColumnResponse>();
        CreateMap<Board, BoardFullResponse>()
            .ForMember(dest => dest.Columns,
                opt => opt.MapFrom(src => src.Columns));
    }
}