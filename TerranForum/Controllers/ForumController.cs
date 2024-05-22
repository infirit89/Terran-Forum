using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
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
        public ForumController(IForumService forumService, IForumRepository forumRepository, 
            ILogger<ForumController> logger, UserManager<ApplicationUser> userManager)
        {
            _ForumService = forumService;
            _ForumRepository = forumRepository;
            _Logger = logger;
            _UserManager = userManager;
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

            ForumThreadViewModel forumThreadViewModel = new ForumThreadViewModel()
            {
                Id = forumId,
                Title = forum.Title,
                Posts = forum.Posts
            };

            return View(forumThreadViewModel);
        }

        [Authorize, HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateThread(CreateForumModel createForumModel)
        {
            createForumModel.UserId = _UserManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _Logger.LogError("Invalid create model");
                return ValidationProblem();
            }
            Forum? forum = await _ForumService.CreateForumThreadAsync(createForumModel);
            if(forum == null)
            {
                _Logger.LogError("Couldn't create forum");
                return Problem("Couldn't create forum");
            }

            return RedirectToAction("ViewThread", "Forum", new
            {
                forumId = forum.Id 
            });
        }

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IForumService _ForumService;
        private readonly IForumRepository _ForumRepository;
        private readonly ILogger<ForumController> _Logger;
        private const int _PageSize = 10;
    }
}
