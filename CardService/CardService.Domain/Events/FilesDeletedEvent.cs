namespace CardService.Domain.Events;

public class FilesDeletedEvent
{
    public List<string> FileIds { get; set; }
}