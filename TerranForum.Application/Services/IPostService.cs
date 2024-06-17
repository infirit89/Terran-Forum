using TerranForum.Application.Dtos.PostDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IPostService
    {
        Task<Post> AddPostToThreadAsync(CreatePostModel createPostModel);
        Task<int> ChangeRatingAsync(UpdatePostRatingModel updatePostRatingModel);
        Task<int> GetUserRatingAsync(string userId, int postId);
        Task DeletePostAsync(DeletePostModel deletePostModel);
        Task<bool> IsMasterPostAsync(int postId);
        Task UpdatePostAsync(UpdatePostModel updatePostModel);
    }
}
