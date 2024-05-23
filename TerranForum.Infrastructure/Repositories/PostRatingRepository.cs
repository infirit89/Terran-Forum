using System.Linq.Expressions;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace TerranForum.Infrastructure.Repositories
{
    internal class PostRatingRepository : IRatingRepository<Post>
    {
        public PostRatingRepository(TerranForumDbContext dbContext) 
        {
            _DbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Rating<Post> model)
        {
            await _DbContext.PostRatings.AddAsync(model);
            return await _DbContext.TrySaveAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<Rating<Post>, bool>> predicate)
        {
            return _DbContext.PostRatings.AnyAsync(predicate);
        }

        public async Task<bool> UpdateAsync(Rating<Post> model)
        {
            _DbContext.Update(model);
            return await _DbContext.TrySaveAsync();
        }

        public Task<Rating<Post>?> GetAsync(string userId, int serviceId)
        {
            return _DbContext.PostRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.ServiceId == serviceId);
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
