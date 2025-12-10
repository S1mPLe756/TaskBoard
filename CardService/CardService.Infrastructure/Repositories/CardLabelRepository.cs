using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using CardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CardService.Infrastructure.Repositories;

public class CardLabelRepository(CardDbContext dbContext) : ICardLabelRepository
{
    public async Task<List<CardLabel>> GetLabelsByCardIdsAsync(List<Guid> cardIds) =>
        await dbContext.Cards.Where(c => cardIds.Contains(c.Id)).SelectMany(x => x.Labels).ToListAsync();
}