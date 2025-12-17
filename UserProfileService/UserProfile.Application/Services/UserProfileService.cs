using System.Net;
using AuthService.Application.DTOs;
using AutoMapper;
using ExceptionService;
using Microsoft.AspNetCore.Http;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Application.Interfaces;
using UserProfile.Domain.Interfaces;
using Profile = UserProfile.Domain.Entities.Profile;

namespace UserProfile.Application.Services;

public class UserProfileService(IProfileRepository profileRepo, IUserPreferencesRepository prefsRepo, IMapper mapper, IFileApiClient fileApiClient)
    : IUserProfileService
{
    public async Task<ProfileDto> CreateProfileAsync(UserProfileDto dto)
    {
       
        var profile = mapper.Map<Profile>(dto);

        await profileRepo.AddAsync(profile);
        return mapper.Map<ProfileDto>(profile);
    }
    
    public async Task<ProfileDto> UpdateProfileAsync(UpdateUserProfileDto dto, Guid userId)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);

        if (profile == null)
        {
            throw new AppException("Profile not found", HttpStatusCode.NotFound);
        }
        
        profile = mapper.Map(dto, profile);    
        
        await profileRepo.UpdateAsync(profile);
        
        return mapper.Map<ProfileDto>(profile);
    }


    public async Task<ProfileDto?> GetProfileAsync(Guid userId)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);
        
        return mapper.Map<ProfileDto>(profile);
    }

    public async Task UpdatePreferencesAsync(UserPreferencesUpdateDto dto)
    {
        var profile = await profileRepo.GetByUserIdAsync(dto.UserId);
        if (profile == null) throw new AppException("Profile not found", HttpStatusCode.NotFound);
        
        profile.Preferences.Language = dto.Language;
        profile.Preferences.NotificationsEnabled = dto.NotificationsEnabled;

        await prefsRepo.UpdateAsync(profile.Preferences);
    }

    public async Task<string> UpdateAvatarAsync(Guid jwtUserId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new AppException("Файл не выбран");

        var user = await profileRepo.GetByUserIdAsync(jwtUserId);

        if (user == null)
            throw new AppException("Profile not found", HttpStatusCode.NotFound);

        if (user.AvatarFileId != null)
        {
            await fileApiClient.DeleteFileAsync(user.AvatarFileId);
        }

        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();

        var response = await fileApiClient.UploadFileAsync(fileStream, file.FileName, file.ContentType);


        user.AvatarFileId = response.Id;

        await profileRepo.UpdateAsync(user);

        return response.Id;
    }
}