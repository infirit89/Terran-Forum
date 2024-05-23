﻿using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;
using TerranForum.Application.Dtos.PostDtos;

namespace TerranForum.Infrastructure.Services
{
    internal class PostService : IPostService
    {
        public PostService(
            IPostRepository postRepository,
            IForumRepository forumRepository,
            IUserRepository userRepository,
            IRatingRepository<Post> postRatingRepository)
        {
            _PostRepository = postRepository;
            _ForumRepository = forumRepository;
            _UserRepository = userRepository;
            _PostRatingRepository = postRatingRepository;
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

        public async Task<bool> ChangeRating(UpdatePostRatingModel updatePostRatingModel)
        {
            if (!await _PostRepository.ExsistsAsync(x => x.Id == updatePostRatingModel.PostId))
                throw new PostNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == updatePostRatingModel.UserId))
                throw new UserNotFoundException();

            Rating<Post> postRating = new()
            {
                UserId = updatePostRatingModel.UserId,
                ServiceId = updatePostRatingModel.PostId,
                Value = updatePostRatingModel.Rating
            };

            if (!await _PostRatingRepository.ExistsAsync(x => x.UserId == postRating.UserId && x.ServiceId == postRating.ServiceId))
                return await _PostRatingRepository.CreateAsync(postRating);

            return await _PostRatingRepository.UpdateAsync(postRating);
        }

        public async Task<int> HasUserRated(string userId, int postId)
        {
            Rating<Post>? postRating = await _PostRatingRepository.GetAsync(userId, postId);
            return postRating != null ? postRating.Value : 0;
        }

        private readonly IPostRepository _PostRepository;
        private readonly IForumRepository _ForumRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IRatingRepository<Post> _PostRatingRepository;
    }
}