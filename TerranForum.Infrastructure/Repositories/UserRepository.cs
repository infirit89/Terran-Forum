using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        public UserRepository(TerranForumDbContext dbContext) 
        {
            _DbContext = dbContext;
        }

        public Task<bool> ExsistsAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return _DbContext.Users.AnyAsync(predicate);
        }

        private readonly TerranForumDbContext _DbContext;
    }
}
