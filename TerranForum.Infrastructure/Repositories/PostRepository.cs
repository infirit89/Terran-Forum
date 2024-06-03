using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TerranForum.Application.Repositories;
using TerranForum.Application.Utils;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        public PostRepository(TerranForumDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Post post)
        {
            await _DbContext.Posts.AddAsync(post);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<bool> DeleteAsync(Post? post)
        {
            if (post is null)
                return false;

            _DbContext.Posts.Remove(post);
            return await _DbContext.TrySaveAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<Post, bool>> predicate, bool withDeleted = false)
        {
            IQueryable<Post> posts = _DbContext.Posts;

            if (withDeleted)
                posts = posts.IgnoreQueryFilters();

            return posts.AnyAsync(predicate);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(
            Expression<Func<Post, bool>>? predicate = null,
            bool withDeleted = false)
        {
            IQueryable<Post> posts = _DbContext.Posts;

            if (withDeleted)
                posts = posts.IgnoreQueryFilters();

            if (predicate != null)
                posts = posts.Where(predicate);

            return await posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllOrderedAsync(
            Expression<Func<Post, bool>>? predicate = null,
            Ordering<Post>? ordering = null)
        {
            IQueryable<Post> posts = _DbContext.Posts;

            if (predicate != null)
                posts = posts.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(posts).ToListAsync();

            return await posts.ToListAsync();
        }

        public Task<Post?> GetByIdAsync(int id, bool withDeleted = false)
        {
            IQueryable<Post> posts = _DbContext.Posts;
            if (withDeleted)
                posts = posts.IgnoreQueryFilters();

            return posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Post? post)
        {
            if (post is null)
                return false;

            _DbContext.Update(post);
            return await _DbContext.TrySaveAsync();
        }

        public Task<Post?> GetFirstWithAsync(Expression<Func<Post, bool>> predicate, params Expression<Func<Post, object>>[] includes)
        {
            IQueryable<Post> posts = _DbContext.Posts;
            return includes
                    .Aggregate(posts, (current, include) => current.Include(include))
                    .FirstOrDefaultAsync(predicate);
        }

        public Task<Post?> GetByIdWithDeletedAsync(int id)
            => GetByIdAsync(id, true);

        public Task<IEnumerable<Post>> GetAllWithDeletedAsync(Expression<Func<Post, bool>>? predicate = null)
            => GetAllAsync(predicate, true);

        public Task<bool> ExistsWithDeletedAsync(Expression<Func<Post, bool>> predicate)
            => ExistsAsync(predicate, true);

        public Task<bool> UndoDeleteAsync(Post model)
        {
            model.IsDeleted = false;
            model.DeletedAt = null;
            return _DbContext.TrySaveAsync();
        }

        public Task<bool> DeleteRangeAsync(IEnumerable<Post> posts)
        {
            _DbContext.RemoveRange(posts);
            return _DbContext.TrySaveAsync();
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
