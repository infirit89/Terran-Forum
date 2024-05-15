using TerranForum.Application.Dtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostService
    {
        Task<Post?> AddPostToThread(CreatePostModel createPostModel);
    }
}
