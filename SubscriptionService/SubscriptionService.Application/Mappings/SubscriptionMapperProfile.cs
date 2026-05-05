using AutoMapper;
using SubscriptionService.Application.DTOs.Responses;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Mappings;

public class SubscriptionMapperProfile : Profile
{
    public SubscriptionMapperProfile()
    {
        CreateMap<Subscription, SubscriptionResponse>();
    }
}