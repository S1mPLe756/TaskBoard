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

    public async Task<Card?> GetCardByIdAsync(Guid id)
    {
        var card = await _context.Cards.Include(c => c.Labels)
            .Include(c => c.Checklist).ThenInclude(c =>c.Items).Include(c=>c.Attachments).FirstOrDefaultAsync(c => c.Id == id);
        if(card is { Checklist: not null })
        {
            card.Checklist.Items = card.Checklist.Items.OrderBy(c => c.Position).ToList();
        }   
        return card;
    }

    public async Task CreateCardAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(Card card)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Card>> GetCardsByIdsAsync(List<Guid> requestCardIds)
    {
        return await _context.Cards.Include(c => c.Labels)
            .Include(c => c.Checklist).ThenInclude(c => c.Items).Where(c => requestCardIds.Contains(c.Id))
            .ToListAsync();
    }

    public async Task DeleteCardsAsync(List<Card> cards)
    {
        _context.Cards.RemoveRange(cards);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(Card card)
    {
        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
    }
}