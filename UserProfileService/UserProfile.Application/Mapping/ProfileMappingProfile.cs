using AuthService.Application.DTOs;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace UserProfile.Application.Mapping;

public class ProfileMappingProfile : Profile
{
    public ProfileMappingProfile()
    {
        CreateMap<UserProfileDto, UserProfile.Domain.Entities.Profile>();
        CreateMap<UserPreferencesUpdateDto, UserPreferences>();

        CreateMap<UserProfile.Domain.Entities.Profile, ProfileDto>().ForMember(
            dest => dest.Preferences, opt => opt.MapFrom(src => src.Preferences));
        CreateMap<UserPreferences, UserPreferencesDto>();

    }
}