using CardService.Domain.Entities;

namespace CardService.Domain.Interfaces;

public interface ICardLabelRepository
{
    Task<List<CardLabel>> GetLabelsByCardIdsAsync(List<Guid> cardIds);
    Task DeleteLabelAsync(CardLabel label);
    Task<CardLabel?> GetLabelByIdAsync(Guid labelId);
}