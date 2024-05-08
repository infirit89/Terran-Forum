using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Application.Utils;
using TerranForum.Domain.Models;
using TerranForum.Models;

namespace TerranForum.Controllers
{
    public class ForumController : Controller
    {
        public ForumController(IForumService forumService, IForumRepository forumRepository, ILogger<ForumController> logger)
        {
            _ForumService = forumService;
            _ForumRepository = forumRepository;
            _Logger = logger;
        }


        public async Task<IActionResult> All(int? page)
        {
            int currentPage = page ?? 0;
            GetForumPagedModel forums =  await _ForumRepository.GetForumsPagedAsync(currentPage, _PageSize);
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageCount"] = forums.PageCount;
            return View(forums.Forums);
        }

        public async Task<IActionResult> ViewThread(int forumId)
        {
            Forum? forum = await _ForumRepository.GetByIdAsync(forumId);
            if (forum == null)
                return NotFound();

            IEnumerable<Post> posts = await _ForumService.GetAllPostsForForum(forum.Id);

            ForumThreadViewModel forumThreadViewModel = new ForumThreadViewModel()
            {
                Title = forum.Title,
                Posts = posts
            };

            return View(forumThreadViewModel);
        }

        private readonly IForumService _ForumService;
        private readonly IForumRepository _ForumRepository;
        private readonly ILogger<ForumController> _Logger;
        private const int _PageSize = 10;
    }
}
