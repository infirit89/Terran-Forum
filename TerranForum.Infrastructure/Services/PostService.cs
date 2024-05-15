using TerranForum.Application.Dtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Services
{
    internal class PostService : IPostService
    {
        public PostService(IPostRepository postRepository) 
        {
            _PostRepository = postRepository;
        }

        public async Task<Post?> AddPostToThread(CreatePostModel createPostModel)
        {
            Post post = new() 
            {
                Content = createPostModel.Content,
                User = createPostModel.User,
                CreatedAt = DateTime.Now,
                Forum = createPostModel.Forum,
                IsMaster = false
            };

            return await _PostRepository.CreateAsync(post) ? post : null;
        }

        private readonly IPostRepository _PostRepository;
    }
}
