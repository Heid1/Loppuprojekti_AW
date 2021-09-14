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
        public SearchController(MoveoContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPostBySportId(int sportid)
        {
                DataAccess da = new DataAccess(_context);
                string id = sportid.ToString();
                var postlist = da.GetPostsByCriteria(id);
                ViewBag.Posts = postlist;
                return View();
        }

        public IActionResult GetPostsByCriteria(string criteria, string button = null)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            ViewBag.Userid = userid;
            DataAccess da = new DataAccess(_context);
            var postlist = new List<Post>();
            if (criteria != null)
            {
                postlist = da.GetPostsByCriteria(criteria);
            }
            else if(userid != null && button == "favouriteSportsButton")
            {
                postlist = da.GetPostsByFavouriteSport((int)userid);
            }
            else if (userid != null && button == "ownPostsButton")
            {
                postlist = da.GetPostsByAttendance((int)userid, true);
            }
            else if (userid != null && button == "attendanceButton")
            {
                postlist = da.GetPostsByAttendance((int)userid, false);
            }
            else
            {
                postlist = da.GetAllPosts();
            }
            ViewBag.Posts = postlist;
            return View();
        }
    }
}
