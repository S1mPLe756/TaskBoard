using AutoMapper;
using CardService.Application.DTOs.Responses;
using CardService.Domain.Entities;

namespace CardService.Application.Mappings;

public class CardAttachmentMapperProfile : Profile
{
    public CardAttachmentMapperProfile()
    {
        CreateMap<CardAttachment, CardAttachmentResponse>();
    }
}