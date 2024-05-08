using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Application.Utils;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Services
{
    internal class ForumService : IForumService
    {
        public ForumService(IForumRepository forumRepository, IPostRepository postRepository) 
        {
            _ForumRepository = forumRepository;
            _PostRepository = postRepository;
        }

        public async Task<bool> CreateForumThreadAsync(CreateForumModel createForumModel)
        {
            Forum forum = new Forum()
            {
                Title = createForumModel.Title
            };

            if (!await _ForumRepository.CreateAsync(forum))
                return false;

            Post masterPost = new Post()
            {
                Content = createForumModel.Content,
                UserId = createForumModel.UserId,
                IsMaster = true,
                Forum = forum,
                CreatedAt = DateTime.Now
            };

            return await _PostRepository.CreateAsync(masterPost);
        }

        private IForumRepository _ForumRepository;
        private IPostRepository _PostRepository;
    }
}
