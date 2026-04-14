using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using CardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CardService.Infrastructure.Repositories;

public class CommentRepository(CardDbContext dbContext) : ICommentRepository
{
    public async Task<Comment?> GetCommentByIdAsync(Guid id)
    {
        return await dbContext.Comments.Include(x=>x.AnsweredComment).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        await dbContext.Comments.AddAsync(comment);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Comment>> GetCommentsCardIdAsync(Guid cardId)
    {
        return await dbContext.Comments.Include(x=>x.AnsweredComment).Where(x=>x.CardId == cardId).ToListAsync();
    }

    public async Task DeleteCommentsAsync(List<Comment> comments)
    {
        dbContext.Comments.RemoveRange(comments);
        await dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteCommentAsync(Comment comment)
    {
        dbContext.Comments.Remove(comment);
        await dbContext.SaveChangesAsync();
    }
}