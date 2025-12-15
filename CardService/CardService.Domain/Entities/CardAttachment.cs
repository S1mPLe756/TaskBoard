namespace CardService.Domain.Entities;

public class CardAttachment
{
    public Guid Id { get; set; }
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}