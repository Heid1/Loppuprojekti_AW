using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Loppuprojekti_AW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoveoContext _context;

        public HomeController(ILogger<HomeController> logger, MoveoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Name) // await missing?
        {
            var user = _context.Endusers.Where(u => u.Username == Name).FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("username", user.Username);
                return View();
            }
            ModelState.AddModelError("Name", "There is no account assosiated with the given name. Please try again or create a new account!");
            return View();

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
    }
}
