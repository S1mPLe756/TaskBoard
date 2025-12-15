namespace CardService.Application.DTOs.Responses;

public class CardAttachmentResponse
{
    public Guid Id { get; set; }
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}