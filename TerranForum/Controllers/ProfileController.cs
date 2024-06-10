using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerranForum.Domain.Models;
using TerranForum.ViewModels.User;

namespace TerranForum.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController(
            UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        [Route("/{userName}")]
        public async Task<IActionResult> ViewProfile(string userName) 
        {
            ApplicationUser user = await _UserManager.FindByNameAsync(userName);
            return View(new UserViewModel
            {
                Username = user.UserName,
                ProfilePictureUrl = user.ProfileImageUrl,
                JoinedAt = user.JoinedAt
            });
        }

        private readonly UserManager<ApplicationUser> _UserManager;
    }
}
