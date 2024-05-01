using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TerranForum.Application.Repositories;
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

        public async Task<bool> ExsistsAsync(Predicate<PostReply> predicate)
        {
            return await _DbContext.PostReplies.AnyAsync(x => predicate(x));
        }

        public async Task<IEnumerable<PostReply>> GetAllAsync(Predicate<PostReply>? predicate = null)
        {
            if (predicate != null)
                return await _DbContext.PostReplies.Where(x => predicate(x)).ToListAsync();

            return await _DbContext.PostReplies.ToListAsync();
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
