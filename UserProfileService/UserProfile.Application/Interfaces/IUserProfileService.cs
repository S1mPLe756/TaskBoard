using AuthService.Application.DTOs;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Domain.Entities;

namespace UserProfile.Application.Interfaces;

public interface IUserProfileService
{
    Task<Profile> CreateProfileAsync(UserProfileDto dto);

    Task<ProfileDto?> GetProfileAsync(Guid userId);

    Task UpdatePreferencesAsync(UserPreferencesUpdateDto dto);
}