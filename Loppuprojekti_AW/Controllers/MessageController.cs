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
                return RedirectToAction("Error", "Home");
            }
            string userName = _data.GetUserById(userId).Username;
            var usersMessagedWith = _data.GetUsersMessagedWith((int)userId);
            var messageHistory = _data.GetMessagesOfUser((int)userId);
            var allUsers = _data.GetAllUsers();
            foreach(var item in allUsers){
                Console.WriteLine(item.Value);
                
            }
            ViewData["currentUserId"] = userId.Value;
            ViewData["currentUserName"] = userName;
            ViewData["usersMessagedWith"] = usersMessagedWith;
            ViewData["messagesHistory"] = messageHistory;
            ViewData["allUsersDict"] = allUsers;
            //ViewBag.userName = _data.GetUserById(userId).Username;
            return View();
        }

        public IActionResult OpenChat()
        {
            int? userId = HttpContext.Session.GetInt32("userid");
            if (userId == null)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.userName = _data.GetUserById(userId).Username;
            ViewData["currentUser"]= _data.GetUserById(userId);
            return View(ViewData);
        }
    }
}
