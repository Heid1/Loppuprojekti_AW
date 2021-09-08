using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Loppuprojekti_AW.Controllers
{
    public class PostController : Controller
    {
        private readonly DataAccess _data;

        public PostController(MoveoContext context)
        {
            _data = new(context);
        }

        // GET: PostController
        public ActionResult Index()
        {
            int userid = (int)HttpContext.Session.GetInt32("userid");
            var organising =  _data.GetPostsByAttendance(userid, true);
            var attending = _data.GetPostsByAttendance(userid, false);
            ViewBag.Attending = attending;
            ViewBag.Organising = organising;
            return View(organising);
        }

        //// GET: PostController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: PostController/Create
        public ActionResult Create()
        {
            ViewBag.Sports = _data.GetAllSports();
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(Post post)
        //{
        //    var userid = HttpContext.Session.GetInt32("userid");
        //    _data.CreatePost((int)userid, post);
        //    return RedirectToAction("Index", "Post");
        //}

        // GET: PostController/Edit/5
        public ActionResult Edit(int postid)
        {
            return View(_data.GetPostById(postid));
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int postid, Post post)
        {
            try
            {
                _data.EditPost(postid, post);
                return RedirectToAction("Index", "Post");
            }
            catch
            {
                return View(_data.GetPostById(postid));
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int postid)
        {
            _data.DeletePost(postid);
            return RedirectToAction("Index", "Post");
        }

        //// POST: PostController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int postid, Post post)
        //{
            
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
