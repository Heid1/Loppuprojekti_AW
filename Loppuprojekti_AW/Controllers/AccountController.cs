using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class AccountController : Controller
    {
        private readonly MoveoContext _context;
        public AccountController(MoveoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Enduser eu)
        {
            DataAccess da = new DataAccess(_context);
            da.CreateUser(eu);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid != null)
            {
                var enduser = DataAccess.GetUserById(userid);
                return View(enduser);
            }
            else
            {
                return RedirectToAction("Virhe", "Home");
            }
        }

        [HttpGet]
        public IActionResult Edit()//siirrytään tiettyyn palveluun uniikin palveluid perusteella, uusi muokkausnäkymä
        {
            var id = HttpContext.Session.GetInt32("userid");
            var user = DataAccess.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            return View(user);
        }


        [HttpPost] //editoidaan henkilöä ja lähetetaan se
        public IActionResult Edit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);
            return View(Eu);

        }

        public IActionResult Delete(Enduser Eu)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var user = DataAccess.GetUserById(userid);
            if (user == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            else
            {
                DataAccess.DeleteUser(user);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }


        }
    }
}
