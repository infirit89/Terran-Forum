using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;

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

        private IForumService _ForumService;
        private IForumRepository _ForumRepository;
        private ILogger<ForumController> _Logger;
        private const int _PageSize = 10;
    }
}
