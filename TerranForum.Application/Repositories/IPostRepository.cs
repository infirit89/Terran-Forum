using System.Linq.Expressions;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post?> GetFirstWithAsync(Expression<Func<Post, bool>> predicate, 
                        params Expression<Func<Post, object>>[] includes);
    }
}
