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

        public async Task<bool> DeleteAsync(Post post)
        {
            _DbContext.Posts.Remove(post);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<bool> ExsistsAsync(Expression<Func<Post, bool>> predicate)
        {
            return await _DbContext.Posts.AnyAsync(predicate);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>>? predicate = null, Ordering<Post>? ordering = null)
        {
            IQueryable<Post> posts = _DbContext.Posts;

            if (predicate != null)
                posts = posts.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(posts).ToListAsync();

            return await posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id) => await _DbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> UpdateAsync(Post post)
        {
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

        private readonly TerranForumDbContext _DbContext;
    }
}
