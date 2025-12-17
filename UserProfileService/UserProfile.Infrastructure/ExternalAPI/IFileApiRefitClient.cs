using Refit;
using UserProfile.Domain.DTOs;

namespace UserProfile.Infrastructure.ExternalAPI;

public interface IFileApiRefitClient
{
    [Post("/api/v1/files")]
    [Multipart]
    Task<FileUploadResponse> UploadFile([AliasAs("file")] StreamPart file);

    [Delete("/api/v1/files/{fileId}")]
    Task DeleteFileAsync(string fileId);
}