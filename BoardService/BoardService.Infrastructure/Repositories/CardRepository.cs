using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using BoardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Repositories;

public class CardRepository(BoardDbContext context) : ICardRepository
{
    public async Task AddCardPositionAsync(CardPosition card)
    {
        await context.CardPositions.AddAsync(card);
        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(Guid columnId)
    {
        return await context.CardPositions.CountAsync(x => x.ColumnId == columnId);
    }

    public async Task<CardPosition?> GetCardPositionByIdAsync(Guid cardId) =>
        await context.CardPositions.Include(c=>c.Column).ThenInclude(cl=>cl.Board).FirstOrDefaultAsync(c => c.CardId == cardId);

    public async Task UpdateCardPositionAsync(CardPosition card)
    {
        context.CardPositions.Update(card);
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(CardPosition card)
    {
        context.CardPositions.Remove(card);
        await context.SaveChangesAsync();
    }
}