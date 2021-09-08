using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile(int Identity)
        {
            var enduser = DataAccess.GetUserById(Identity);

            return View(enduser);
        }

        public IActionResult ProfileEdit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);

            return View(Eu);
        }

    }
}
