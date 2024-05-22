using System.Linq.Expressions;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExsistsAsync(Expression<Func<ApplicationUser, bool>> predicate);
    }
}
