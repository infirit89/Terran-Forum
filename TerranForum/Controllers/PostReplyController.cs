using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Services;
using TerranForum.Domain.Exceptions;
using TerranForum.Application.Dtos.PostReplyDtos;
using Microsoft.AspNetCore.Identity;
using TerranForum.Domain.Models;
using TerranForum.Application.Repositories;
using TerranForum.ViewModels.PostReply;

namespace TerranForum.Controllers
{
    public class PostReplyController : Controller
    {
        public PostReplyController(
            ILogger<PostReplyController> logger,
            IPostReplyService postReplyService,
            IPostRepository postRepository,
            UserManager<ApplicationUser> userManager)
        {
            _Logger = logger;
            _PostReplyService = postReplyService;
            _PostRepository = postRepository;
            _UserManager = userManager;
        }

        public IActionResult GetPostReplyDeleteView(DeletePostReplyViewModel deletePostReplyViewModel)
        {
            return PartialView(
                "~/Views/PostReply/_DeletePartial.cshtml",
                deletePostReplyViewModel);
        }

        public async Task<IActionResult> Delete(DeletePostReplyViewModel deletePostReplyViewModel) 
        {
            try
            {
                await _PostReplyService.DeletePostReplyAsync(new DeletePostReplyModel
                {
                    PostId = deletePostReplyViewModel.PostId,
                    ReplyId = deletePostReplyViewModel.ReplyId,
                    UserId = _UserManager.GetUserId(User)
                });

                Post post = (await _PostRepository.GetByIdAsync(deletePostReplyViewModel.PostId))!;
                return RedirectToAction("ViewThread", "Forum", new { ForumId = post.ForumId });
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError("Couldn't delete forum");
                return StatusCode(500);
            }
        }

        private readonly IPostReplyService _PostReplyService;
        private readonly IPostRepository _PostRepository;
        private readonly ILogger<PostReplyController> _Logger;
        private readonly UserManager<ApplicationUser> _UserManager;
    }
}
