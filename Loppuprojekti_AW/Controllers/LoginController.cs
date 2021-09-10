using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;

namespace Loppuprojekti_AW.Controllers
{
    public class LoginController : Controller
    {
        private readonly MoveoContext _context;
        public LoginController(MoveoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Email, string Password)
        {
            Enduser Eu = _context.Endusers.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();

            if (Eu != null)
            {
                HttpContext.Session.SetInt32("userid", Eu.Userid);
                HttpContext.Session.SetString("username", Eu.Username);
                HttpContext.Session.SetString("email", Eu.Email);
                HttpContext.Session.SetString("userrole", Eu.Userrole);
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("Username", "There is no account assosiated with the given name. Please try again or create a new account!");
                ViewBag.AuthOK = false;
                return View();
            }
        }
    }
}
