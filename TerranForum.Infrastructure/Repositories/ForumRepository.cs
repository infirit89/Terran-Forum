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

        public async Task<Forum?> GetByIdAsync(int id)
        {
            return await _DbContext.Forums
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Ratings)
                        .ThenInclude(r => r.User)
                .Select(f => new Forum
                {
                    Id = f.Id,
                    Title = f.Title,
                    Posts = f.Posts
                    .OrderByDescending(p => p.IsMaster)
                    .ThenByDescending(p => p.CreatedAt)
                    .Select(p => new Post 
                    {
                        Id = p.Id,
                        Content = p.Content,
                        UserId = p.UserId,
                        User = p.User,
                        CreatedAt = p.CreatedAt,
                        Replies = p.Replies.OrderByDescending(pr => pr.CreatedAt),
                        ForumId = p.ForumId,
                        Forum = p.Forum,
                        IsMaster = p.IsMaster,
                        Ratings = p.Ratings.ToArray()
                    })
                })
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> UpdateAsync(Forum forum)
        {
            _DbContext.Update(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<ForumsPagedModel> GetForumsPagedAsync(int page, int size) 
        {
            return new ForumsPagedModel()
            {
                Data = await _DbContext.Forums.Skip(page * size).Take(size).ToListAsync(),
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
