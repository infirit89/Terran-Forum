using System.Linq.Expressions;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IPostReplyRepository : IRepository<PostReply>
    {
        Task<PostReply?> GetFirstAsync(Expression<Func<PostReply, bool>> predicate);
    }
}
