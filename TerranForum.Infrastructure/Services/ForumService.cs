using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;
using TerranForum.Application.Dtos.PostDtos;

namespace TerranForum.Infrastructure.Services
{
    internal class ForumService : IForumService
    {
        public ForumService(
            IForumRepository forumRepository,
            IPostRepository postRepository,
            IUserRepository userRepository,
            IPostReplyRepository postReplyRepository)
        {
            _ForumRepository = forumRepository;
            _PostRepository = postRepository;
            _UserRepository = userRepository;
            _PostReplyRepository = postReplyRepository;
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
                throw new CreateModelException();

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

            throw new CreateModelException();
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

        public async Task DeleteForumThread(int forumId, string userId)
        {
            Forum? forum = await _ForumRepository.GetFirstWithAsync(x => x.Id == forumId, x => x.Posts);

            if (forum == null)
                throw new ForumNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == userId))
                throw new UserNotFoundException();

            //Post masterPost = await _PostRepository
            //    .GetFirstWithAsync(
            //        x => x.ForumId == forumId && x.IsMaster && x.UserId == userId, x => x.Replies)
            //    ?? throw new PostNotFoundException();

            //foreach (var reply in masterPost.Replies)
            //{
            //    if (!await _PostReplyRepository.DeleteAsync(reply))
            //        throw new DeleteModelException();
            //}

            //if (!await _PostRepository.DeleteAsync(masterPost))
            //    throw new DeleteModelException();

            foreach (var post in forum.Posts)
            {
                if (!await _PostRepository.DeleteAsync(post))
                    throw new DeleteModelException();
            }

            if (!await _ForumRepository.DeleteAsync(forum))
                throw new DeleteModelException();
        }

        private IForumRepository _ForumRepository;
        private IPostRepository _PostRepository;
        private IUserRepository _UserRepository;
        private IPostReplyRepository _PostReplyRepository;
    }
}
