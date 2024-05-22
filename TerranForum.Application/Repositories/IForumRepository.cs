using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IForumRepository : IRepository<Forum>
    {
        Task<GetForumPagedModel> GetForumsPagedAsync(int page, int size);
    }
}
