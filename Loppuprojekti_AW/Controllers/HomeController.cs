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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username)
        {
            var user = _context.Endusers.Where(u => u.Username == Username).FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetInt32("userid", user.Userid);
                HttpContext.Session.SetInt32("userrole", user.Userrole);
                return View();
            }
            ModelState.AddModelError("Username", "There is no account assosiated with the given name. Please try again or create a new account!");
            return RedirectToAction("Index");

        }

        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetInt32("userid");
            var enduser = DataAccess.GetUserById();

            return View(enduser);
        }

        public IActionResult AddPost()
        {
           
            return View();
        }


        public IActionResult ProfileEdit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);

            return View(Eu);
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
