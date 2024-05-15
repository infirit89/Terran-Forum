using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Services
{
    internal class PostReplyService : IPostReplyService
    {
        public PostReplyService(IPostReplyRepository postReplyRepository) 
        {
            _PostReplyRepository = postReplyRepository;
        }

        public async Task<PostReply?> AddPostReply(CreatePostReplyModel createPostReplyModel)
        {
            PostReply postReply = new() 
            {
                Content = createPostReplyModel.Content,
                CreatedAt = DateTime.Now,
                User = createPostReplyModel.User,
                Post = createPostReplyModel.Post
            };

            return await _PostReplyRepository.CreateAsync(postReply) ? postReply : null;
        }

        private readonly IPostReplyRepository _PostReplyRepository;
    }
}
