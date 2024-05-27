using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ExistsAsync(Expression<Func<PostReply, bool>> predicate, bool withDeleted = false)
        {
            IQueryable<PostReply> postReplies = _DbContext.PostReplies;

            if (withDeleted)
                postReplies = postReplies.IgnoreQueryFilters();

            return await postReplies.AnyAsync(predicate);
        }

        public async Task<IEnumerable<PostReply>> GetAllAsync(Expression<Func<PostReply, bool>>? predicate = null, bool withDeleted = false)
        {
            IQueryable<PostReply> postReplies = _DbContext.PostReplies;

            if (withDeleted)
                postReplies = postReplies.IgnoreQueryFilters();

            if (predicate != null)
                postReplies = postReplies.Where(predicate);

            return await postReplies.ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetAllOrderedAsync(Expression<Func<PostReply, bool>>? predicate = null, Ordering<PostReply>? ordering = null)
        {
            IQueryable<PostReply> postReplies = _DbContext.PostReplies;

            if (predicate != null)
                postReplies = postReplies.Where(predicate);

            if (ordering != null)
                return await ordering.Apply(postReplies).ToListAsync();

            return await postReplies.ToListAsync();
        }

        public async Task<PostReply?> GetByIdAsync(int id, bool withDeleted = false)
        {
            IQueryable<PostReply> postReplies = _DbContext.PostReplies;

            if (withDeleted)
                postReplies = postReplies.IgnoreQueryFilters();

            return await postReplies.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<bool> UpdateAsync(PostReply postReply)
        {
            _DbContext.Update(postReply);
            return await _DbContext.TrySaveAsync();
        }

        public Task<PostReply?> GetByIdWithDeletedAsync(int id)
            => GetByIdAsync(id, true);

        public Task<IEnumerable<PostReply>> GetAllWithDeletedAsync(Expression<Func<PostReply, bool>>? predicate = null)
            => GetAllAsync(predicate, true);

        public Task<bool> ExistsWithDeletedAsync(Expression<Func<PostReply, bool>> predicate)
            => ExistsAsync(predicate, true);

        public Task<bool> UndoDeleteAsync(PostReply model)
        {
            model.IsDeleted = false;
            model.DeletedAt = null;
            return _DbContext.TrySaveAsync();
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
