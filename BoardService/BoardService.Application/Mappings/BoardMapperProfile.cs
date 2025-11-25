using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Domain.Entities;
using BoardService.Domain.ValueObjects;

namespace BoardService.Application.Mappings;

public class BoardMapperProfile : Profile
{
    public BoardMapperProfile()
    {
        CreateMap<CreateBoardRequest, Board>();
        CreateMap<Board, BoardResponse>();
        CreateMap<BoardColumn, BoardColumnResponse>();
        CreateMap<ColumnCard, ColumnCardResponse>();

    }
}