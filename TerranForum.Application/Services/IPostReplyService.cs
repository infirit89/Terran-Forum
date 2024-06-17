using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostReplyService
    {
        Task<PostReply> AddPostReplyAsync(CreatePostReplyModel createPostReplyModel);
        Task DeletePostReplyAsync(DeletePostReplyModel deletePostReplyModel);
    }
}
