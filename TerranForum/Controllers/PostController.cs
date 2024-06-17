using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos.PostDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Exceptions;
using TerranForum.Domain.Models;
using TerranForum.ViewModels.Post;
using TerranForum.ViewModels.PostReply;

namespace TerranForum.Controllers
{
    public class PostController : Controller
    {
        public PostController(
            ILogger<PostController> logger,
            UserManager<ApplicationUser> userManager,
            IPostReplyService postReplyService,
            IPostService postService,
            IPostRepository postRepository)
        {
            _Logger = logger;
            _UserManager = userManager;
            _PostReplyService = postReplyService;
            _PostService = postService;
            _PostRepository = postRepository;
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CreatePostCommentViewModel createPostCommentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _Logger.LogError("Invalid create model");
                return ValidationProblem();
            }

            try
            {
                await _PostReplyService.AddPostReplyAsync(new()
                {
                    Content = createPostCommentViewModel.Content,
                    UserId = _UserManager.GetUserId(User),
                    PostId = createPostCommentViewModel.PostId
                });
            }
            catch (TerranForumException)
            {
                _Logger.LogError("Couldn't create post reply");
                return StatusCode(500);
            }

            return RedirectToAction("ViewThread", "Forum", new
            {
                forumId = createPostCommentViewModel.ForumId
            });
        }

        [HttpGet, Authorize]
        public IActionResult GetCreatePostReplyView(int forumId, int postId)
        {
            return PartialView(
                "~/Views/PostReply/_CreatePartial.cshtml",
                new CreatePostCommentViewModel()
                {
                    PostId = postId,
                    ForumId = forumId
                });
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostViewModel createPostViewModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(500);

            CreatePostModel createPostModel = new()
            {
                UserId = _UserManager.GetUserId(User),
                ForumId = createPostViewModel.ForumId,
                Content = createPostViewModel.Content
            };

            try
            {
                await _PostService.AddPostToThreadAsync(createPostModel);
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return RedirectToAction("ViewThread", "Forum", new
            {
                forumId = createPostViewModel.ForumId
            });
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateRating(int postId, sbyte rating)
        {
            try
            {
                if (!(rating >= -1 && rating <= 1))
                    return StatusCode(500);

                int newRating = await _PostService.ChangeRatingAsync(new UpdatePostRatingModel()
                {
                    UserId = _UserManager.GetUserId(User),
                    PostId = postId,
                    Rating = rating
                });
                return Json(new { Rating = newRating });
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetPostDeleteView(DeletePostViewModel model)
        {
            try
            {
                model.IsMaster = await _PostService.IsMasterPostAsync(model.PostId);

                return PartialView(
                    "~/Views/Post/_DeletePartial.cshtml",
                    model);
            }
            catch (PostNotFoundException) 
            {
                _Logger.LogError("Couldn't find the post to delete");
                return StatusCode(404);
            }
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePostViewModel model) 
        {
            try
            {
                if (await _PostService.IsMasterPostAsync(model.PostId))
                    return StatusCode(500);

                await _PostService.DeletePostAsync(new DeletePostModel
                {
                    UserId = _UserManager.GetUserId(User),
                    PostId = model.PostId,
                    ForumId = model.ForumId
                });

                return RedirectToAction("ViewThread", "Forum", new { ForumId = model.ForumId });
            }
            catch (TerranForumException)
            {
                _Logger.LogError("Couldn't delete post");
                return StatusCode(500);
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Edit(int postId) 
        {
            Post? post = await _PostRepository
                .GetFirstWithAsync(
                    x => x.Id == postId
                    && x.UserId == _UserManager.GetUserId(User));

            if (post is null)
                return StatusCode(404);

            // you can't edit master post with this controller
            if (post.IsMaster)
                return StatusCode(403);

            return View(new EditPostViewModel
            {
                PostId = postId,
                Content = post.Content,
                ForumId = post.ForumId
            });
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel) 
        {
            try
            {
                if (await _PostService.IsMasterPostAsync(editPostViewModel.PostId))
                    return StatusCode(500);

                await _PostService.UpdatePostAsync(new UpdatePostModel
                {
                    PostId = editPostViewModel.PostId,
                    UserId = _UserManager.GetUserId(User),
                    PostContent = editPostViewModel.Content
                });

                return RedirectToAction("ViewThread", "Forum", new { ForumId = editPostViewModel.ForumId });
            }
            catch (TerranForumException)
            {
                _Logger.LogError("Couldn't update the post");
                return StatusCode(500);
            }
        }

        private readonly ILogger<PostController> _Logger;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IPostReplyService _PostReplyService;
        private readonly IPostService _PostService;
        private readonly IPostRepository _PostRepository;
    }
}
