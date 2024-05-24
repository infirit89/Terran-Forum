using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TerranForum.Models;

namespace TerranForum.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            ILogger<HomeController> logger)
        {
            _Logger = logger;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly ILogger<HomeController> _Logger;
    }
}
