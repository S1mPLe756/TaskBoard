namespace BoardService.Domain.DTOs;

public class GetCardsBatchRequest
{
    public List<Guid> CardIds { get; set; } = [];
}