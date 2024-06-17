using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Exceptions;
using TerranForum.Domain.Models;
using TerranForum.ViewModels.Forum;
using TerranForum.ViewModels.Post;
using TerranForum.ViewModels.PostReply;

namespace TerranForum.Controllers
{
    public class ForumController : Controller
    {
        public ForumController(
            IForumService forumService,
            IForumRepository forumRepository,
            ILogger<ForumController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _ForumService = forumService;
            _ForumRepository = forumRepository;
            _Logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> All(int? page, string? q)
        {
            if (_SignInManager.IsSignedIn(User) && await _UserManager.GetUserAsync(User) == null) 
            {
                await _SignInManager.SignOutAsync();
                return RedirectToAction("All");
            }

            int currentPage = page ?? 0;
            ForumsPagedModel pagedForums;

            if(q is null)
                pagedForums =  await _ForumRepository
                    .GetForumsPagedAsync(currentPage, _PageSize);
            else
                pagedForums = await _ForumRepository
                    .GetForumsPagedAsync(currentPage, _PageSize, x => x.Title.ToLower().Contains(q.ToLower()));

            ViewData["CurrentPage"] = currentPage;
            ViewData["PageCount"] = pagedForums.PageCount;
            ViewData["ForumCount"] = pagedForums.Data.Count();
            IEnumerable<Task<ForumViewModel>> forumData =
                pagedForums.Data
                .Select(
                    async f => await ConvertToForumViewModel(f));
            return View(forumData);
        }

        private async Task<ForumViewModel> ConvertToForumViewModel(Forum forum) 
        {
            Post masterPost = await _ForumService.GetForumMasterPostAsync(forum.Id);
            string masterPostContent = new string(masterPost.Content
                            .Take(_MasterPostContentThumbnailSize)
                            .ToArray());

            if (masterPost.Content.Length >= _MasterPostContentThumbnailSize + 3)
                masterPostContent += new string('.', 3);

            return new ForumViewModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Rating = masterPost.Ratings.Sum(r => r.Value),
                User = masterPost.User,
                Content = masterPostContent,
                CreatorUserName = masterPost.User.UserName
            };
        }

        [HttpGet]
        public async Task<IActionResult> ViewThread(int forumId)
        {
            Forum? forum = await _ForumRepository.GetByIdWithAllAsync(forumId);
            if (forum == null)
                return NotFound();

            ForumThreadViewModel forumThreadViewModel = new ForumThreadViewModel()
            {
                Id = forumId,
                Title = forum.Title,
                Posts = forum.Posts.Select(p => new PostViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ForumId = p.ForumId,
                    CreatorUserName = p.User.UserName,
                    Rating = p.Ratings.Sum(x => x.Value),
                    Content = p.Content,
                    IsMaster = p.IsMaster,
                    CurrentUserRating = (p.Ratings
                    .FirstOrDefault(x => x.UserId == _UserManager.GetUserId(User)) 
                    ?? new Rating<Post>()).Value,
                    Replies = p.Replies.Select(pr => new PostReplyViewModel
                    {
                        Id = pr.Id,
                        PostId = pr.PostId,
                        Content = pr.Content,
                        UserId = pr.UserId,
                        Username = pr.User.UserName
                    })
                })
            };

            return View(forumThreadViewModel);
        }

        [Authorize, HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateThread(CreateForumViewModel createForumViewModel)
        {
            if (!ModelState.IsValid)
            {
                _Logger.LogError("Invalid create model");
                return ValidationProblem();
            }

            try
            {
                Forum forum = await _ForumService.CreateForumThreadAsync(new CreateForumModel 
                {
                    Title = createForumViewModel.Title,
                    Content = createForumViewModel.Content,
                    UserId = _UserManager.GetUserId(User)
                });
                return RedirectToAction("ViewThread", "Forum", new
                {
                    forumId = forum.Id
                });
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError("Couldn't create thread");
                return StatusCode(500);
            }
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int forumId) 
        {
            try
            {
                await _ForumService.DeleteForumThreadAsync(forumId, _UserManager.GetUserId(User));
                return RedirectToAction("All", "Forum");
            }
            catch (TerranForumException ex)
            {
                _Logger.LogError("Couldn't delete forum");
                return StatusCode(500);
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Edit(int forumId)
        {
            try
            {
                ForumDataModel forumData = await _ForumService.GetForumDataAsync(forumId, _UserManager.GetUserId(User));
                return View(new UpdateForumViewModel 
                {
                    Title = forumData.Title,
                    Content = forumData.Content,
                    ForumId = forumId
                });
            }
            catch (ModelNotFoundException)
            {
                _Logger.LogError("Couldn't find the forum/master post");
                return StatusCode(404);
            }
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateForumViewModel updateForumViewModel) 
        {
            try
            {
                await _ForumService.UpdateForumThreadAsync(new UpdateForumModel
                {
                    ForumId = updateForumViewModel.ForumId,
                    UserId = _UserManager.GetUserId(User),
                    Title = updateForumViewModel.Title,
                    Content = updateForumViewModel.Content
                });

                return RedirectToAction("ViewThread", "Forum", new { updateForumViewModel.ForumId });
            }
            catch (TerranForumException)
            {
                _Logger.LogError("Couldn't update the forum");
                return StatusCode(500);
            }
        }

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IForumService _ForumService;
        private readonly IForumRepository _ForumRepository;
        private readonly ILogger<ForumController> _Logger;
        private const int _PageSize = 10;
        private const int _MasterPostContentThumbnailSize = 250;
        private readonly SignInManager<ApplicationUser> _SignInManager;
    }
}
