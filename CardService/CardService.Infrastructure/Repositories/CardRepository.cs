using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using CardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Card>> GetCardsByIdsAsync(List<Guid> requestCardIds)
    {
        return await _context.Cards.Include(c=>c.Labels)
            .Include(c=>c.Checklists).ThenInclude(c=>c.Items).Where(c => requestCardIds.Contains(c.Id)).ToListAsync();
    }

    public async Task DeleteCardsAsync(List<Card> cards)
    {
        _context.Cards.RemoveRange(cards);
        await _context.SaveChangesAsync();
    }
}