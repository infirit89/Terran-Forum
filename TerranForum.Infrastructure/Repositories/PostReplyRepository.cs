using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using TerranForum.Application.Repositories;
using TerranForum.Application.Utils;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Repositories
{
    public class PostReplyRepository : IPostReplyRepository
    {
        public PostReplyRepository(TerranForumDbContext dbContext) 
        {
            _DbContext = dbContext;
        }

        public async Task<bool> CreateAsync(PostReply postReply)
        {
            await _DbContext.PostReplies.AddAsync(postReply);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<bool> DeleteAsync(PostReply postReply)
        {
            _DbContext.PostReplies.Remove(postReply);
            return await _DbContext.TrySaveAsync();
        }

        public async Task<bool> ExsistsAsync(Expression<Func<PostReply, bool>> predicate)
        {
            return await _DbContext.PostReplies.AnyAsync(predicate);
        }

        public async Task<IEnumerable<PostReply>> GetAllAsync(Expression<Func<PostReply, bool>>? predicate = null, Ordering<PostReply>? ordering = null)
        {
            IQueryable<PostReply> postReplies = _DbContext.PostReplies;

            if (predicate != null)
                postReplies = postReplies.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(postReplies).ToListAsync();

            return await postReplies.ToListAsync();
        }

        public async Task<PostReply?> GetByIdAsync(int id) => await _DbContext.PostReplies.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> UpdateAsync(PostReply postReply)
        {
            _DbContext.Update(postReply);
            return await _DbContext.TrySaveAsync();
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
