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
        public IActionResult ProfileCreate(Enduser eu)
        {
            DataAccess da = new DataAccess(_context);
            da.CreateUser(eu);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile(int userid)
        {
            var enduser = DataAccess.GetUserById(userid);
            return View(enduser);
        }

        public IActionResult ProfileEdit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);
            return View(Eu);
        }
        public IActionResult ProfileDelete()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var user =  DataAccess.GetUserById(userid);
            DataAccess.DeleteUser(user);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
