namespace FileService.Domain.Events;

public class FileUploadEvent
{
    public string FileId { get; set; }
    public string FileName { get; set; }
}