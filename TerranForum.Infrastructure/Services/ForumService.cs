using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TerranForum.Tests")]

namespace TerranForum.Infrastructure.Services
{
    internal class ForumService : IForumService
    {
        public ForumService(
            IForumRepository forumRepository,
            IPostRepository postRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _ForumRepository = forumRepository;
            _PostRepository = postRepository;
            _UserRepository = userRepository;
            _UserService = userService;
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

        public async Task<Post> GetForumMasterPostAsync(int forumId) 
        {
            Post post = await _PostRepository
                .GetFirstWithAsync(p =>
                    p.ForumId == forumId && p.IsMaster,
                    p => p.Ratings, p => p.User)
                        ?? throw new PostNotFoundException();
            return post;
        }

        public async Task DeleteForumThreadAsync(int forumId, string userId)
        {
            if (!await _UserRepository.ExsistsAsync(x => x.Id == userId))
                throw new UserNotFoundException();


            if (!await _UserService.IsUserAdminAsync(userId))
            {
                if (!await _PostRepository.ExistsAsync(x => x.ForumId == forumId && x.IsMaster && x.UserId == userId))
                    throw new PostNotFoundException();
            }
            else
            {
                if (!await _PostRepository.ExistsAsync(x => x.ForumId == forumId && x.IsMaster))
                    throw new PostNotFoundException();
            }

            Forum forum = await _ForumRepository
                .GetFirstWithAsync(
                    x => x.Id == forumId)
                ?? throw new ForumNotFoundException();


            // NOTE: maybe delete master post as well?
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

            if (!await _ForumRepository.DeleteAsync(forum))
                throw new DeleteModelException();
        }

        public async Task UpdateForumThreadAsync(UpdateForumModel updateForumModel)
        {
            if (!await _UserRepository.ExsistsAsync(x => x.Id == updateForumModel.UserId))
                throw new UserNotFoundException();

            Post masterPost = await _PostRepository
                .GetFirstWithAsync(x =>
                    x.ForumId == updateForumModel.ForumId
                    && x.IsMaster
                    && x.UserId == updateForumModel.UserId)
                ?? throw new PostNotFoundException();

            Forum forum = await _ForumRepository.GetByIdAsync(updateForumModel.ForumId) 
                ?? throw new ForumNotFoundException();

            masterPost.Content = updateForumModel.Content;
            if (!await _PostRepository.UpdateAsync(masterPost))
                throw new UpdateModelException();

            forum.Title = updateForumModel.Title;
            if (!await _ForumRepository.UpdateAsync(forum))
                throw new UpdateModelException();
        }

        public async Task<ForumDataModel> GetForumDataAsync(int forumId, string userId)
        {
            Post masterPost = await _PostRepository
                .GetFirstWithAsync(x => 
                    x.ForumId == forumId
                    && x.IsMaster
                    && x.UserId == userId)
                ?? throw new PostNotFoundException();

            Forum forum = await _ForumRepository.GetByIdAsync(forumId)
                ?? throw new ForumNotFoundException();

            return new ForumDataModel
            {
                Title = forum.Title,
                Content = masterPost.Content
            };
        }

        private IForumRepository _ForumRepository;
        private IPostRepository _PostRepository;
        private IUserRepository _UserRepository;
        private IUserService _UserService;
    }
}
