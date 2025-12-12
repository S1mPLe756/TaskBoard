using BoardService.Domain.Entities;

namespace BoardService.Domain.Interfaces;

public interface ICardRepository
{
    Task AddCardPositionAsync(CardPosition card);
    
    Task<int> CountAsync(Guid columnId);

    Task<CardPosition?> GetCardPositionByIdAsync(Guid cardId);
    Task UpdateCardPositionAsync(CardPosition card);
    Task SaveChangesAsync();
}