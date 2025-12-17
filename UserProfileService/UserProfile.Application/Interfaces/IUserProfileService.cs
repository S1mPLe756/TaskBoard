using AuthService.Application.DTOs;
using Microsoft.AspNetCore.Http;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Domain.Entities;

namespace UserProfile.Application.Interfaces;

public interface IUserProfileService
{
    Task<ProfileDto> CreateProfileAsync(UserProfileDto dto);

    Task<ProfileDto?> GetProfileAsync(Guid userId);

    Task<ProfileDto> UpdateProfileAsync(UpdateUserProfileDto dto, Guid userId);
    
    Task UpdatePreferencesAsync(UserPreferencesUpdateDto dto);
    Task<string> UpdateAvatarAsync(Guid jwtUserId, IFormFile file);
}