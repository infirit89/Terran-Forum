using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Exceptions;
using TerranForum.Application.Dtos.PostDtos;
using System.Linq.Expressions;

namespace TerranForum.Infrastructure.Services
{
    internal class PostService : IPostService
    {
        public PostService(
            IPostRepository postRepository,
            IForumRepository forumRepository,
            IUserRepository userRepository,
            IRatingRepository<Post> postRatingRepository,
            IUserService userService)
        {
            _PostRepository = postRepository;
            _ForumRepository = forumRepository;
            _UserRepository = userRepository;
            _PostRatingRepository = postRatingRepository;
            _UserService = userService;
        }

        public async Task<Post> AddPostToThreadAsync(CreatePostModel createPostModel)
        {
            if (!await _ForumRepository.ExistsAsync(x => x.Id == createPostModel.ForumId))
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

            throw new CreateModelException();
        }

        public async Task<int> ChangeRatingAsync(UpdatePostRatingModel updatePostRatingModel)
        {
            if (!await _PostRepository.ExistsAsync(x => x.Id == updatePostRatingModel.PostId))
                throw new PostNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == updatePostRatingModel.UserId))
                throw new UserNotFoundException();

            Rating<Post>? postRating = await _PostRatingRepository.GetAsync(updatePostRatingModel.UserId, updatePostRatingModel.PostId);
            Post post = 
                await _PostRepository
                .GetFirstWithAsync(
                    x => x.Id == updatePostRatingModel.PostId,
                    x => x.Ratings) 
                ?? throw new PostNotFoundException();

            if (postRating != null)
            {
                if (postRating.Value == updatePostRatingModel.Rating)
                {
                    postRating.Value = (sbyte)(postRating.Value + -postRating.Value);
                }
                else 
                {
                    postRating.Value = updatePostRatingModel.Rating;
                }

                if (!await _PostRatingRepository.UpdateAsync(postRating))
                    throw new UpdateModelException();

                return post.Ratings.Sum(r => r.Value);
            }

            postRating = new()
            {
                UserId = updatePostRatingModel.UserId,
                ServiceId = updatePostRatingModel.PostId,
                Value = updatePostRatingModel.Rating
            };

            if (!await _PostRatingRepository.CreateAsync(postRating))
                throw new CreateModelException();

            return post.Ratings.Sum(r => r.Value);
        }

        public async Task<int> GetUserRatingAsync(string userId, int postId)
        {
            Rating<Post>? postRating = await _PostRatingRepository.GetAsync(userId, postId);
            return postRating != null ? postRating.Value : 0;
        }

        public async Task DeletePostAsync(DeletePostModel deletePostModel)
        {
            if (!await _ForumRepository.ExistsAsync(x => x.Id == deletePostModel.ForumId))
                throw new ForumNotFoundException();

            if (!await _UserRepository.ExsistsAsync(x => x.Id == deletePostModel.UserId))
                throw new UserNotFoundException();

            Expression<Func<Post, bool>> predicate;

            if (await _UserService.IsUserAdminAsync(deletePostModel.UserId))
                predicate = (x) =>
                    x.Id == deletePostModel.PostId
                        && x.ForumId == deletePostModel.ForumId;
            else
                predicate = x => x.Id == deletePostModel.PostId
                        && x.ForumId == deletePostModel.ForumId
                        && x.UserId == deletePostModel.UserId;

            Post post = post = await _PostRepository
                    .GetFirstWithAsync(predicate)
                    ?? throw new PostNotFoundException();

            if (!await _PostRepository.DeleteAsync(post))
                throw new DeleteModelException();
        }

        public async Task<bool> IsMasterPostAsync(int postId)
        {
            Post post = await _PostRepository.GetByIdAsync(postId) ??
                throw new PostNotFoundException();

            return post.IsMaster;
        }

        public async Task UpdatePostAsync(UpdatePostModel updatePostModel)
        {
            if (!await _UserRepository.ExsistsAsync(x => x.Id == updatePostModel.UserId))
                throw new UserNotFoundException();

            Post post = await _PostRepository
                .GetFirstWithAsync(
                    x => x.Id == updatePostModel.PostId
                    && x.UserId == updatePostModel.UserId)
                ?? throw new PostNotFoundException();

            post.Content = updatePostModel.PostContent;
            if (!await _PostRepository.UpdateAsync(post))
                throw new UpdateModelException();
        }

        private readonly IPostRepository _PostRepository;
        private readonly IForumRepository _ForumRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IRatingRepository<Post> _PostRatingRepository;
        private readonly IUserService _UserService;
    }
}
