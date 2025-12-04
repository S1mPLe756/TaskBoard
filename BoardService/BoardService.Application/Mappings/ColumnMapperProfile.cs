using AutoMapper;
using BoardService.Application.DTOs;
using BoardService.Domain.Entities;

namespace BoardService.Application.Mappings;

public class ColumnMapperProfile : Profile
{
    public ColumnMapperProfile()
    {
        CreateMap<CreateColumnBoardRequest, BoardColumn>();
    }
}