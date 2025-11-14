using AutoMapper;
using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Mappings;

public class NotificationMapperProfile : Profile
{
    public NotificationMapperProfile()
    {
        CreateMap<NotificationRequest, Notification>();
    }
}