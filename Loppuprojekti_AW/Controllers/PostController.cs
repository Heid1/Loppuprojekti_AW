﻿using Loppuprojekti_AW.Models;
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

        //// GET: PostController
        public ActionResult Index()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            var organising = _data.GetPostsByAttendance((int)userid, true);
            var attending = _data.GetPostsByAttendance((int)userid, false);
            ViewBag.Attending = attending;
            ViewBag.Organising = organising;
            return View(organising);
        }

        //// GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            ViewBag.Sports = _data.GetAllSports();
            return View();
        }

        // POST: PostController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            _data.CreatePost((int)userid, post);
            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public ActionResult Attend(int postid)
        {
            var userid = (int)HttpContext.Session.GetInt32("userid");
            _data.AttendPost(userid, postid);
            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public ActionResult CancelAttendance(int postid)
        {
            var userid = (int)HttpContext.Session.GetInt32("userid");
            _data.CancelAttendance(userid, postid);
            return RedirectToAction("Index", "Post");
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int postid)
        {
            ViewBag.Sports = _data.GetAllSports();
            return View(_data.GetPostById(postid));
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            try
            {
                _data.EditPost(post);
                return RedirectToAction("Index", "Post");
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int postid)
        {
            _data.DeletePost(postid);
            return RedirectToAction("Index", "Post");
        }
    }
}
