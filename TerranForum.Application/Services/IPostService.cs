using TerranForum.Application.Dtos.PostDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostService
    {
        Task<Post> AddPostToThread(CreatePostModel createPostModel);
        Task<bool> ChangeRating(UpdatePostRatingModel updatePostRatingModel);
        Task<int> GetUserRating(string userId, int postId);
    }
}
