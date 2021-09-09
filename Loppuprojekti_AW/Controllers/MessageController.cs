using Loppuprojekti_AW.Models;
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
        public IActionResult Index(int userId)
        {
            ViewData["messages"] = _data.GetMessagesOfUser(userId);
            ViewData["users"] = _data.GetUsersMessagedWith(userId);
            return View(ViewData);
            //return View(_data.GetMessagesOfUser(userId));
        }
    }
}
