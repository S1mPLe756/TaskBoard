namespace FileService.Application.DTOs;

public class FileUploadRequest
{
    public byte[] Content { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}