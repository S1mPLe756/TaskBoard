namespace CardService.Application.DTOs;

public class GetCardsBatchRequest
{
    public List<Guid> CardIds { get; set; } = [];
}