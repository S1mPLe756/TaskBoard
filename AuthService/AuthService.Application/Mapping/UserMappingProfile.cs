using AuthService.Application.DTOs.Responses;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>();
    }
}