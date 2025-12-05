using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using CardService.Infrastructure.DbContext;

namespace CardService.Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
    private CardDbContext _context;

    public CardRepository(CardDbContext context)
    {
        _context = context;
    }

    public async Task<Card?> GetCardByIdAsync(Guid id) => await _context.Cards.FindAsync(id);

    public async Task CreateCardAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(Card card)
    {
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();
    }
}