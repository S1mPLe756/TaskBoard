namespace CardService.Application.DTOs.Requests;

public class GetCardsBatchRequest
{
    public List<Guid> CardIds { get; set; } = [];
}