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
            IUserRepository userRepository)
        {
            _PostReplyRepository = postReplyRepository;
            _PostRepository = postRepository;
            _UserRepository = userRepository;
        }

        public async Task<PostReply> AddPostReply(CreatePostReplyModel createPostReplyModel)
        {
            if (!await _PostRepository.ExsistsAsync(x => x.Id == createPostReplyModel.PostId))
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

            throw new CantCreateModelException();
        }

        private readonly IPostReplyRepository _PostReplyRepository;
        private readonly IPostRepository _PostRepository;
        private readonly IUserRepository _UserRepository;
    }
}
