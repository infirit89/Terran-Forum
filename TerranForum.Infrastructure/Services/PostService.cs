using TerranForum.Application.Dtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;

namespace TerranForum.Infrastructure.Services
{
    internal class PostService : IPostService
    {
        public PostService(
            IPostRepository postRepository,
            IForumRepository forumRepository,
            IUserRepository userRepository)
        {
            _PostRepository = postRepository;
            _ForumRepository = forumRepository;
            _UserRepository = userRepository;
        }

        public async Task<Post> AddPostToThread(CreatePostModel createPostModel)
        {
            if (!await _ForumRepository.ExsistsAsync(x => x.Id == createPostModel.ForumId))
                throw new ForumNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == createPostModel.UserId))
                throw new UserNotFoundException();

            Post post = new() 
            {
                Content = createPostModel.Content,
                UserId = createPostModel.UserId,
                CreatedAt = DateTime.Now,
                ForumId = createPostModel.ForumId,
                IsMaster = false
            };

            if (await _PostRepository.CreateAsync(post))
                return post;

            throw new CantCreateModelException();
        }

        private readonly IPostRepository _PostRepository;
        private readonly IForumRepository _ForumRepository;
        private readonly IUserRepository _UserRepository;
    }
}
