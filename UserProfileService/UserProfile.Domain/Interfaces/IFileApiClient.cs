using UserProfile.Domain.DTOs;

namespace UserProfile.Domain.Interfaces;

public interface IFileApiClient
{
    Task<FileUploadResponse> UploadFileAsync(Stream file, String fileName, String contentType);
    Task DeleteFileAsync(string fileId);
}