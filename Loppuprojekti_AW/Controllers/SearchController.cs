using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class SearchController : Controller
    {
        private readonly MoveoContext _context;
        private readonly DataAccess _data;

        public SearchController(MoveoContext context)
        {
            _context = context;
            _data = new DataAccess(context);

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPostBySportId(int sportid)
        {
                string id = sportid.ToString();
                var postlist = _data.GetPostsByCriteria(id);
                ViewBag.Posts = postlist;
                return View();
        }

        public IActionResult GetPostsByCriteria(string criteria, string button = null)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            ViewBag.Userid = userid;
            //ViewBag.Attendance = _data.GetAttendances(userid).
            var postlist = new List<Post>();
            if (criteria != null)
            {
                postlist = _data.GetPostsByCriteria(criteria);
            }
            else if(userid != null && button == "favouriteSportsButton")
            {
                postlist = _data.GetPostsByFavouriteSport((int)userid);
            }
            else if (userid != null && button == "ownPostsButton")
            {
                postlist = _data.GetPostsByAttendance((int)userid, true);
            }
            else if (userid != null && button == "attendanceButton")
            {
                postlist = _data.GetPostsByAttendance((int)userid, false);
            }
            else
            {
                postlist = _data.GetAllPosts();
            }
            //// Tarkistetaan osallistujien määrä
            //ViewBag.currentParCount = db.Participants.Where(e => e.EventId == id).ToList().Count();
            //ViewBag.parCountFull = false;
            //if (ViewBag.currentParCount == ev.Participants)
            //{
            //    ViewBag.parCountFull = true;
            //}
            //// Tässä tsekataan onko sivun selaaja tapahtuman järjestäjä tai osallistuja, valmis.
            //var par = (db.Participants).Where(e => (e.EventId == id && e.PersonId == personId)).FirstOrDefault();
            //ViewBag.Partisipant = par;
            //if (par != null)
            //{
            //    ViewBag.Organizer = par.Organizer;
            //}
            return View(postlist);
        }
    }
}
