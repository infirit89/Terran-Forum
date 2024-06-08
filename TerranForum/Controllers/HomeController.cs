using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TerranForum.ViewModels;

namespace TerranForum.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            ILogger<HomeController> logger)
        {
            _Logger = logger;
        }


        public IActionResult Index()
        {
            return RedirectToAction("All", "Forum");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode is not null) 
            {
                switch (statusCode) 
                {
                    case 404:
                    case 500:
                        return View($"Errors/Error{statusCode}");
                }
            }

            return View("Errors/Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly ILogger<HomeController> _Logger;
    }
}
