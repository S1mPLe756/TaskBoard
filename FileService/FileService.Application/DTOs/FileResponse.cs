namespace FileService.Application.DTOs;

public class FileResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Url { get; set; }
}