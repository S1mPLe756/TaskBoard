using CardService.Domain.Entities;

namespace CardService.Domain.Interfaces;

public interface ICardRepository
{
    Task<Card?> GetCardByIdAsync(Guid id);
    Task CreateCardAsync(Card card);
    Task UpdateCardAsync(Card card);
}