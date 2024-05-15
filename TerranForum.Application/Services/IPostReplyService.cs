using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostReplyService
    {
        Task<PostReply?> AddPostReply(CreatePostReplyModel createPostReplyModel);
    }
}
