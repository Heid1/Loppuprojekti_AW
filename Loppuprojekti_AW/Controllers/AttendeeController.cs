using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly MoveoContext _context;
        public AttendeeController(MoveoContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Enroll(int userid)
        {
            DataAccess da = new DataAccess(_context);
            
            Attendee newattendee = new Attendee();
            newattendee.Userid = userid;
            newattendee.Organiser = false;

            Post attendingpost = new Post();
            attendingpost.Postid = newattendee.Postid;
            return RedirectToAction("Index", "Account");
        }

        //create new attendee
        //jossa postiin lisätään uusi attendee
    }
}
