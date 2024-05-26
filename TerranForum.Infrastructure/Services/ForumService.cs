using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;

namespace TerranForum.Infrastructure.Services
{
    internal class ForumService : IForumService
    {
        public ForumService(
            IForumRepository forumRepository,
            IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _ForumRepository = forumRepository;
            _PostRepository = postRepository;
            _UserRepository = userRepository;
        }

        public async Task<Forum> CreateForumThreadAsync(CreateForumModel createForumModel)
        {
            if (!await _UserRepository.ExsistsAsync(x => x.Id == createForumModel.UserId))
                throw new UserNotFoundException();

            Forum forum = new Forum()
            {
                Title = createForumModel.Title
            };

            if (!await _ForumRepository.CreateAsync(forum))
                throw new CantCreateModelException();

            Post masterPost = new Post()
            {
                Content = createForumModel.Content,
                UserId = createForumModel.UserId,
                IsMaster = true,
                Forum = forum,
                CreatedAt = DateTime.Now
            };

            if (await _PostRepository.CreateAsync(masterPost))
                return forum;

            throw new CantCreateModelException();
        }

        public async Task<Post> GetForumMasterPost(int forumId) 
        {
            Post post = await _PostRepository
                .GetFirstWithAsync(p =>
                    p.ForumId == forumId && p.IsMaster,
                    p => p.Ratings, p => p.User)
                        ?? throw new PostNotFoundException();
            return post;
        }

        private IForumRepository _ForumRepository;
        private IPostRepository _PostRepository;
        private IUserRepository _UserRepository;
    }
}
