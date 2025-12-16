namespace FileService.Domain.Events;

public class FilesDeleteEvent
{
    public List<string> FileIds { get; set; }
}