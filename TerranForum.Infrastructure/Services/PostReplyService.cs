using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;

namespace TerranForum.Infrastructure.Services
{
    internal class PostReplyService : IPostReplyService
    {
        public PostReplyService(
            IPostReplyRepository postReplyRepository,
            IPostRepository postRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _PostReplyRepository = postReplyRepository;
            _PostRepository = postRepository;
            _UserRepository = userRepository;
            _UserService = userService;
        }

        public async Task<PostReply> AddPostReply(CreatePostReplyModel createPostReplyModel)
        {
            if (!await _PostRepository.ExistsAsync(x => x.Id == createPostReplyModel.PostId))
                throw new PostNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == createPostReplyModel.UserId))
                throw new UserNotFoundException();

            PostReply postReply = new()
            {
                Content = createPostReplyModel.Content,
                CreatedAt = DateTime.Now,
                UserId = createPostReplyModel.UserId,
                PostId = createPostReplyModel.PostId
            };

            if (await _PostReplyRepository.CreateAsync(postReply))
                return postReply;

            throw new CreateModelException();
        }

        public async Task DeletePostReplyAsync(DeletePostReplyModel deletePostReplyModel)
        {
            if(!await _PostRepository.ExistsAsync(x => x.Id == deletePostReplyModel.PostId))
                throw new PostNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == deletePostReplyModel.UserId))
                throw new UserNotFoundException();

            PostReply postReply;

            if (!await _UserService.IsUserAdmin(deletePostReplyModel.UserId))
            {
                postReply = await _PostReplyRepository
                    .GetFirstAsync(
                        x => x.Id == deletePostReplyModel.ReplyId
                        && x.PostId == deletePostReplyModel.PostId
                        && x.UserId == deletePostReplyModel.UserId)
                    ?? throw new PostReplyNotFoundException();
            }
            else
            {
                postReply = await _PostReplyRepository
                    .GetFirstAsync(
                        x => x.Id == deletePostReplyModel.ReplyId
                        && x.PostId == deletePostReplyModel.PostId)
                    ?? throw new PostReplyNotFoundException();
            }

            if (!await _PostReplyRepository.DeleteAsync(postReply))
                throw new DeleteModelException();
        }

        private readonly IPostReplyRepository _PostReplyRepository;
        private readonly IPostRepository _PostRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IUserService _UserService;
    }
}
