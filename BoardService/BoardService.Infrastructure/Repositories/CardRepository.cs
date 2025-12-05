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
}