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

        public Task<Post?> GetFirstWithRatingAsync(Expression<Func<Post, bool>> predicate)
        {
            return _DbContext.Posts.Include(p => p.Ratings).FirstOrDefaultAsync(predicate);
        }

        public Task<Post?> GetFirstWithUserAsync(Expression<Func<Post, bool>> predicate)
        {
            return _DbContext.Posts.Include(p => p.User).FirstOrDefaultAsync(predicate);
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
