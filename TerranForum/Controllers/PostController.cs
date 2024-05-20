using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos.PostReplyDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
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

            Post? post = await _PostRepository.GetByIdAsync(createPostCommentViewModel.PostId);

            if (post == null) 
            {
                _Logger.LogError("Invalid post");
                return StatusCode(500);
            }

            await _PostReplyService.AddPostReply(new()
            {
                Content = createPostCommentViewModel.Content,
                UserId = _UserManager.GetUserId(User),
                PostId = createPostCommentViewModel.PostId
            });


            return RedirectToAction("ViewThread", "Forum", new
            {
                forumId = post.ForumId
            });
        }

        public IActionResult GetCreatePostReplyView(int postId) 
        {
            return PartialView(
                "~/Views/PostReply/_CreatePostReplyPartial.cshtml",
                new CreatePostCommentViewModel()
                {
                    PostId = postId
                });
        }

        public IActionResult Index()
        {
            return View();
        }

        private readonly ILogger<PostController> _Logger;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IPostReplyService _PostReplyService;
        private readonly IPostRepository _PostRepository;
    }
}
