namespace FileService.Domain.Interfaces;

public interface IFileService
{
    Task<string> UploadAsync(byte[] content, string fileName, string contentType);
    Task<byte[]> DownloadAsync(string fileId);
    Task DeleteAsync(string fileId);
    Task<IEnumerable<FileService.Domain.Entities.File>> ListAsync();
    Task<string> GetFileUrl(string fileId);
}