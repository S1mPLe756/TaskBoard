using UserProfile.Application.DTOs.Requestes;
using UserProfile.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace UserProfile.Application.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<UserProfileDto, UserProfile.Domain.Entities.Profile>();
        CreateMap<UserPreferencesUpdateDto, UserPreferences>();
    }
}