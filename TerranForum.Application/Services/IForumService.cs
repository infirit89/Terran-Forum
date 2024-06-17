using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IForumService
    {
        Task<Forum> CreateForumThreadAsync(CreateForumModel createForumModel);
        Task<Post> GetForumMasterPostAsync(int forumId);
        Task DeleteForumThreadAsync(int forumId, string userId);
        Task UpdateForumThreadAsync(UpdateForumModel updateForumModel);
        Task<ForumDataModel> GetForumDataAsync(int forumId, string userId);
    }
}
