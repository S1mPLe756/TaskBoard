using AutoMapper;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class CommentMapperProfile : Profile
{
    public CommentMapperProfile()
    {
        CreateMap<CreateCommentRequest, Comment>();
        CreateMap<Comment, CommentResponse>()
            .ForMember(x=>x.AnsweredComment, opt => opt.MapFrom(dest=>dest.AnsweredComment));
    }
}