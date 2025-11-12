using System.Net;
using AutoMapper;
using ExceptionService;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Application.Interfaces;
using UserProfile.Domain.Interfaces;
using Profile = UserProfile.Domain.Entities.Profile;

namespace UserProfile.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IProfileRepository _profileRepo;
    private readonly IUserPreferencesRepository _prefsRepo;
    private readonly IMapper _mapper;

    public UserProfileService(IProfileRepository profileRepo, IUserPreferencesRepository prefsRepo, IMapper mapper)
    {
        _profileRepo = profileRepo;
        _prefsRepo = prefsRepo;
        _mapper = mapper;
    }

    public async Task<Profile> CreateProfileAsync(UserProfileDto dto)
    {
       
        var profile = _mapper.Map<Profile>(dto);

        await _profileRepo.AddAsync(profile);
        return profile;
    }

    public async Task<Profile?> GetProfileAsync(Guid userId)
        => await _profileRepo.GetByUserIdAsync(userId);

    public async Task UpdatePreferencesAsync(UserPreferencesUpdateDto dto)
    {
        var profile = await _profileRepo.GetByUserIdAsync(dto.UserId);
        if (profile == null) throw new AppException("Profile not found", HttpStatusCode.NotFound);
        
        profile.Preferences.Language = dto.Language;
        profile.Preferences.NotificationsEnabled = dto.NotificationsEnabled;

        await _prefsRepo.UpdateAsync(profile.Preferences);
    }

}