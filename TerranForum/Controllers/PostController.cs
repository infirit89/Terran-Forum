using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Domain.Models;
using TerranForum.Models;

namespace TerranForum.Controllers
{
    public class PostController : Controller
    {
        public PostController(ILogger<PostController> logger, UserManager<ApplicationUser> userManager)
        {
            _Logger = logger;
            _UserManager = userManager;
        }

        [HttpPost, Authorize]
        public IActionResult AddComment(CreatePostCommentViewModel createPostCommentViewModel) 
        {
            createPostCommentViewModel.UserId = _UserManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                _Logger.LogError("Invalid create model");
                return ValidationProblem();
            }

            _Logger.LogTrace(createPostCommentViewModel.Content);
            _Logger.LogTrace(createPostCommentViewModel.PostId.ToString());

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        private readonly ILogger<PostController> _Logger;
        private readonly UserManager<ApplicationUser> _UserManager;
    }
}
