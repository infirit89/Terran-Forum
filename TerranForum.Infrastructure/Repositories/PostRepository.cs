using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Repositories;
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

        public async Task<IEnumerable<Post>> GetAllAsync(Predicate<Post>? predicate = null)
        {
            if (predicate != null)
                return await _DbContext.Posts.Where(x => predicate(x)).ToListAsync();

            return await _DbContext.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id) => await _DbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> UpdateAsync(Post post)
        {
            _DbContext.Update(post);
            return await _DbContext.TrySaveAsync();
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
