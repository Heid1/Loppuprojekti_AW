using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

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
            Enduser enduser = _context.Endusers.Where(u => u.Email == Email).FirstOrDefault();

            if (enduser != null)
            {
                if (Validate(Password, enduser.Password))
                {
                    HttpContext.Session.SetInt32("userid", enduser.Userid);
                    HttpContext.Session.SetString("username", enduser.Username);
                    HttpContext.Session.SetString("email", enduser.Email);
                    HttpContext.Session.SetString("userrole", enduser.Userrole);
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ViewBag.AuthOK = false;
                    return View();
                }
            }
            else
            {
                ViewBag.AuthOK = false;
                return View();
            }
        }
        /// <summary>
        /// Muokkaa salasanan eri muotoon, jottei näy suoraan tietokannassa.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Hash(string value)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes("Passw0rd"),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }
        public static bool Validate(string value, string hash)
            => Hash(value) == hash;
    }
}
