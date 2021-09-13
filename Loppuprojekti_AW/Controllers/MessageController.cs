using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class MessageController : Controller
    {
        private readonly DataAccess _data;

        public MessageController(MoveoContext context)
        {
            _data = new(context);
            
        }
        public IActionResult Index()
        {
            //ViewData["messages"] = _data.GetMessagesOfUser(userId);
            //ViewData["users"] = _data.GetUsersMessagedWith(userId);
            int? userId = HttpContext.Session.GetInt32("userid");
            if (userId == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            string userName = _data.GetUserById(userId).Username;
            var usersMessagedWith = _data.GetUsersMessagedWith((int)userId);

            ViewData["currentUserId"] = userId.Value;
            ViewData["currentUserName"] = userName;
            ViewData["usersMessagedWith"] = usersMessagedWith;
            //ViewData["messagesHistory"] = _data.GetMessagesOfUser((int)userId);
            //ViewBag.userName = _data.GetUserById(userId).Username;
            return View();
        }

        public IActionResult OpenChat()
        {
            int? userId = HttpContext.Session.GetInt32("userid");
            if (userId == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            ViewBag.userName = _data.GetUserById(userId).Username;
            ViewData["currentUser"]= _data.GetUserById(userId);
            return View(ViewData);
        }
    }
}
