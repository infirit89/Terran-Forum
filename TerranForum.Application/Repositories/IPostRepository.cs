using System.Linq.Expressions;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post?> GetFirstWithRatingAsync(Expression<Func<Post, bool>> predicate);
        Task<Post?> GetFirstWithUserAsync(Expression<Func<Post, bool>> predicate);
    }
}
