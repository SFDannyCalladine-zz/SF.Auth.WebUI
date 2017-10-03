using Microsoft.AspNetCore.Mvc;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Auth.WebUI.Models;
using System.Diagnostics;

namespace SF.Auth.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;

        public HomeController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            var connectionResponse = _authService.FindConnectionByEmail("darrell@storefeeder.com");

            var user = _authService.ValidateUser(new ValidateUserRequest("darrell@storefeeder.com", "Opensesam3"));

            var kauser = _authService.ValidateUser(new ValidateUserRequest("support@kukoon.com", "Opensesam3"));

            return View();
        }
    }
}