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

        public async Task<bool> DeleteAsync(Forum? forum)
        {
            if (forum is null)
                return false;

            _DbContext.Forums.Remove(forum);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<IEnumerable<Forum>> GetAllAsync(Expression<Func<Forum, bool>>? predicate = null, bool withDeleted = false)
        {
            IQueryable<Forum> forums = _DbContext.Forums;

            if (withDeleted)
                forums = forums.IgnoreQueryFilters();

            if (predicate != null)
                forums = forums.Where(predicate);

            return await forums.ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetAllOrderedAsync(Expression<Func<Forum, bool>>? predicate = null, Ordering<Forum>? ordering = null) 
        {
            IQueryable<Forum> forums = _DbContext.Forums;
            if (predicate != null)
                forums = forums.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(forums).ToListAsync();

            return await forums.ToListAsync();
        }

        public Task<Forum?> GetByIdAsync(int id, bool withDeleted = false)
        {
            IQueryable<Forum> forums = _DbContext.Forums;
            if (withDeleted)
                forums = forums.IgnoreQueryFilters();

            return forums.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Forum? forum)
        {
            if (forum is null)
                return false;

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

        public async Task<bool> ExistsAsync(Expression<Func<Forum, bool>> predicate, bool withDeleted = false)
        {
            IQueryable<Forum> forums = _DbContext.Forums;

            if (withDeleted)
                forums = forums.IgnoreQueryFilters();

            return await forums.AnyAsync(predicate);
        }

        public Task<Forum?> GetByIdWithDeletedAsync(int id) => GetByIdAsync(id, true);
        public Task<IEnumerable<Forum>> GetAllWithDeletedAsync(Expression<Func<Forum, bool>>? predicate = null)
            => GetAllAsync(predicate, true);

        public Task<bool> ExistsWithDeletedAsync(Expression<Func<Forum, bool>> predicate)
            => ExistsAsync(predicate, true);

        public Task<bool> UndoDeleteAsync(Forum model)
        {
            model.IsDeleted = false;
            model.DeletedAt = null;
            return _DbContext.TrySaveAsync();
        }

        public Task<Forum?> GetByIdWithAllAsync(int forumId, bool withDeleted = false)
        {
            IQueryable<Forum> forums = _DbContext.Forums;

            if (withDeleted)
                forums = forums.IgnoreQueryFilters();

            return forums
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
                .FirstOrDefaultAsync(f => f.Id == forumId);
        }

        public Task<Forum?> GetFirstWithAsync(Expression<Func<Forum, bool>> predicate, params Expression<Func<Forum, object>>[] includes)
        {
            IQueryable<Forum> forums = _DbContext.Forums;
            return includes
                    .Aggregate(forums, (current, include) => current.Include(include))
                    .FirstOrDefaultAsync(predicate);
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
