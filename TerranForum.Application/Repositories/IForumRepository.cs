using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IForumRepository : IRepository<Forum>
    {
        Task<ForumsPagedModel> GetForumsPagedAsync(int page, int size);
    }
}
