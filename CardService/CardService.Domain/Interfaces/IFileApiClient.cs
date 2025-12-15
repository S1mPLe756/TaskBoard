using CardService.Domain.DTOs;

namespace CardService.Domain.Interfaces;

public interface IFileApiClient
{
    Task<FileUploadResponse> UploadFileAsync(Stream file, String fileName, String contentType);
}