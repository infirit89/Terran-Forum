using Microsoft.AspNetCore.Mvc;

namespace TerranForum.Controllers
{
    public class ProfileController : Controller
    {
        [Route("/{userName}")]
        public IActionResult ViewProfile(string userName) 
        {
            ViewData["Username"] = userName;
            return View();
        }
    }
}
