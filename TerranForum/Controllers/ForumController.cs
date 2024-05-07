using Microsoft.AspNetCore.Mvc;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;

namespace TerranForum.Controllers
{
    public class ForumController : Controller
    {
        public ForumController(IForumService forumService, IForumRepository forumRepository)
        {
            _ForumService = forumService;
            _ForumRepository = forumRepository;
        }


        public async Task<IActionResult> All() 
        {
            IEnumerable<Forum> forums =  await _ForumRepository.GetForumsPaged(_CurrentPage, 10);
            return View(forums);
        }

        [HttpPost]
        public IActionResult NextPage() 
        {
            _CurrentPage++;
            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult PreviousPage() 
        {
            _CurrentPage--;
            return RedirectToAction("All");
        }

        public IActionResult Index()
        {
            return View();
        }

        private IForumService _ForumService;
        private IForumRepository _ForumRepository;
        private int _CurrentPage = 0;
    }
}
