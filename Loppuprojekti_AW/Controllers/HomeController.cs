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
            //DataAccess data = new DataAccess(_context);
            //data.ReturnCoordinates("Rakuunantie 17");
            //return View();

            DataAccess da = new DataAccess(_context);
            var prevalencelist = da.GetAllSports();
            ViewBag.CommonPosts = prevalencelist;
            return View();
        }
        public IActionResult Index2()
        {
            DataAccess da = new DataAccess(_context);
            ViewBag.ilmoitukset = da.

            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            Enduser Eu = _context.Endusers.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();

            if (Eu != null)
            {
                HttpContext.Session.SetInt32("userid", Eu.Userid);
                HttpContext.Session.SetString("username", Eu.Username);
                HttpContext.Session.SetString("userrole", Eu.Userrole);
                return RedirectToAction("Index", "Account");
            }
            else
            {
                //ModelState.AddModelError("Username", "There is no account assosiated with the given name. Please try again or create a new account!");
                ViewBag.AuthOK = false;
                return View();
            }

        }

        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetInt32("userid");
            var enduser = DataAccess.GetUserById(id);

            return View(enduser);
        }

        public IActionResult ProfileEdit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);

            return View(Eu);
        }

        // NÄIHIN EN KEKSINYT HELPPOA RATKAISUA
        //public IActionResult SendFeedback()
        //{
        //    return View();
        //}
        //public IActionResult SendFeedback(string text)
        //{

        //    return View();
        //}

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
