using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IForumService
    {
        Task<Forum> CreateForumThreadAsync(CreateForumModel createForumModel);
    }
}
