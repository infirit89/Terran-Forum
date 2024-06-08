using System.Linq.Expressions;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IForumRepository : IRepository<Forum>
    {
        Task<ForumsPagedModel> GetForumsPagedAsync(int page, int size, Expression<Func<Forum, bool>>? predicate = null);
        // this will do select so no change tracking
        Task<Forum?> GetByIdWithAllAsync(int forumId, bool withDeleted = false);
        Task<Forum?> GetFirstWithAsync(Expression<Func<Forum, bool>> predicate,
                        params Expression<Func<Forum, object>>[] includes);
    }
}
