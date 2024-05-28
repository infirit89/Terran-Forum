using TerranForum.Application.Dtos.PostDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostService
    {
        Task<Post> AddPostToThread(CreatePostModel createPostModel);
        Task<int> ChangeRating(UpdatePostRatingModel updatePostRatingModel);
        Task<int> GetUserRating(string userId, int postId);
        Task DeletePost(DeletePostModel deletePostModel);
        Task<bool> IsMasterPost(int postId);
        Task<bool> UpdatePost(UpdatePostModel updatePostModel);
    }
}
