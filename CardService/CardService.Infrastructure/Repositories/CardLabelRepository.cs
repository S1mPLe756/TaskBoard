using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using CardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CardService.Infrastructure.Repositories;

public class CardLabelRepository(CardDbContext dbContext) : ICardLabelRepository
{
    public async Task<List<CardLabel>> GetLabelsByCardIdsAsync(List<Guid> cardIds) =>
        await dbContext.Cards.Where(c => cardIds.Contains(c.Id)).SelectMany(x => x.Labels).Distinct().ToListAsync();

    public async Task DeleteLabelAsync(CardLabel label)
    {
        dbContext.CardLabels.Remove(label);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CardLabel?> GetLabelByIdAsync(Guid labelId)
    {
        return await dbContext.CardLabels.Include(c=>c.Cards).FirstOrDefaultAsync(x => x.Id == labelId);
    }
}