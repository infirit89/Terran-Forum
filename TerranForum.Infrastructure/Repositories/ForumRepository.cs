using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Utils;
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

        public async Task<IEnumerable<Forum>> GetAllAsync(Expression<Func<Forum, bool>>? predicate = null, Ordering<Forum>? ordering = null)
        {
            IQueryable<Forum> forums = _DbContext.Forums;
            if (predicate != null)
                forums = forums.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(forums).ToListAsync();

            return await forums.ToListAsync();
        }

        public async Task<Forum?> GetByIdAsync(int id) => await _DbContext.Forums.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> UpdateAsync(Forum forum)
        {
            _DbContext.Update(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<GetForumPagedModel> GetForumsPagedAsync(int page, int size) 
        {
            return new GetForumPagedModel()
            {
                Forums = await _DbContext.Forums.Skip(page * size).Take(size).ToListAsync(),
                PageCount = Math.Max(await _DbContext.Forums.CountAsync() / size, 1)
            };
        }

        public async Task<bool> ExsistsAsync(Expression<Func<Forum, bool>> predicate)
        {
            return await _DbContext.Forums.AnyAsync(predicate);
        }
        
        private readonly TerranForumDbContext _DbContext;
    }
}
