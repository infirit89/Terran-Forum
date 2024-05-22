using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos;
using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Exceptions;
using TerranForum.Domain.Models;
using TerranForum.Models;

namespace TerranForum.Controllers
{
    public class PostController : Controller
    {
        public PostController(
            ILogger<PostController> logger,
            UserManager<ApplicationUser> userManager,
            IPostReplyService postReplyService,
            IPostRepository postRepository)
        {
            _Logger = logger;
            _UserManager = userManager;
            _PostReplyService = postReplyService;
            _PostRepository = postRepository;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> AddComment(CreatePostCommentViewModel createPostCommentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _Logger.LogError("Invalid create model");
                return ValidationProblem();
            }

            try
            {
                await _PostReplyService.AddPostReply(new()
                {
                    Content = createPostCommentViewModel.Content,
                    UserId = _UserManager.GetUserId(User),
                    PostId = createPostCommentViewModel.PostId
                });
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError("Couldn't create post reply");
                return StatusCode(500);
            }

            return RedirectToAction("ViewThread", "Forum", new
            {
                forumId = createPostCommentViewModel.ForumId
            });
        }

        [HttpGet]
        public IActionResult GetCreatePostReplyView(int forumId, int postId)
        {
            return PartialView(
                "~/Views/PostReply/_CreatePostReplyPartial.cshtml",
                new CreatePostCommentViewModel()
                {
                    PostId = postId,
                    ForumId = forumId
                });
        }

        [HttpPost]
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
                await _PostService.AddPostToThread(createPostModel);
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

        private readonly ILogger<PostController> _Logger;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IPostReplyService _PostReplyService;
        private readonly IPostRepository _PostRepository;
        private readonly IPostService _PostService;
    }
}
