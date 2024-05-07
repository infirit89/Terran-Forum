using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Repositories
{
    public class ForumRepository : IForumRepository
    {
        public ForumRepository(TerranForumDbContext dbContext) 
        {
            _DbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Forum forum)
        {
            await _DbContext.Forums.AddAsync(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<bool> DeleteAsync(Forum forum)
        {
            _DbContext.Forums.Remove(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<IEnumerable<Forum>> GetAllAsync(Predicate<Forum>? predicate = null)
        {
            if (predicate != null)
                return await _DbContext.Forums.Where(x => predicate(x)).ToListAsync();

            return await _DbContext.Forums.ToListAsync();
        }

        public async Task<Forum?> GetByIdAsync(int id) => await _DbContext.Forums.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> UpdateAsync(Forum forum)
        {
            _DbContext.Update(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsPaged(int page, int size) 
        {
            return await _DbContext.Forums.Skip(page * size).Take(size).ToListAsync();
        }

        public async Task<bool> ExsistsAsync(Expression<Func<Forum, bool>> predicate)
        {
            return await _DbContext.Forums.AnyAsync(predicate);
        }
        
        private readonly TerranForumDbContext _DbContext;
    }
}
