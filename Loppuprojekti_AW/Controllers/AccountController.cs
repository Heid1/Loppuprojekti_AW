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
            ViewBag.Nimi = HttpContext.Session.GetString("username");
            return View();
        }

        public IActionResult Logout()
        {
                HttpContext.Session.Clear();
                return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Epäonnistui = false;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Enduser eu)
        {
            DataAccess da = new DataAccess(_context);
            bool result = da.GetUsersByEmail(eu.Email); //boolin result on joko false tai true

            if (result == false) //jos result on sama kuin false eli vastaavuutta ei löytynyt
            {
                da.CreateUser(eu);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Epäonnistui = true;
                return View();
            }
        }
    

    [HttpGet]
    public IActionResult Profile()
    {
        var userid = HttpContext.Session.GetInt32("userid");
        if (userid != null)
        {
            var enduser = new DataAccess(_context).GetUserById(userid);
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
        var userid = HttpContext.Session.GetInt32("userid");
        var enduser = new DataAccess(_context).GetUserById(userid);
        if (enduser == null)
        {
            return RedirectToAction("Virhe", "Home");
        }
        return View(enduser);
    }


    [HttpPost] //editoidaan henkilöä ja lähetetaan se
    public IActionResult Edit(Enduser Eu)
    {
        DataAccess da = new DataAccess(_context);
        da.EditUser(Eu);
        return RedirectToAction("Profile", new { Id = Eu.Userid });
    }

    public IActionResult Delete(Enduser Eu)
    {
        var userid = HttpContext.Session.GetInt32("userid");
        DataAccess da = new DataAccess(_context);
        var enduser = da.GetUserById(userid);
        if (enduser == null)
        {
            return RedirectToAction("Virhe", "Home");
        }
        else
        {
            da.DeleteUser(enduser);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
}
