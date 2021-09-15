using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    public class SportController : Controller
    {
        private readonly DataAccess _data;


        public SportController(MoveoContext context)
        {
            _data = new(context);
        }

        // GET: SportController
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            ViewBag.Userid = userid;
            ViewBag.Userrole = HttpContext.Session.GetString("userrole");
            List<int> userssports = null;
            if (userid != null)
            {
                userssports = _data.FindUsersSports(userid).Select(s => s.Sportid).ToList();
            }
            ViewBag.IsChosen = userssports;
            return View(_data.GetAllSports());
        }

        // GET: SportController/Details/5
        public IActionResult Details(int sportid)
        {
            return View(_data.GetSportById(sportid));
        }

        // GET: SportController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(Sport sport)
        {
            _data.CreateSport(sport);
            return RedirectToAction("Index", "Sport");
        }

        // GET: SportController/Edit/5
        public IActionResult Edit(int sportid)
        {
            return View(_data.GetSportById(sportid));
        }

        // POST: SportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sport sport)
        {
            _data.EditSport(sport);
            return View("Index");
        }

        // GET: SportController/Delete/5
        public IActionResult Delete(int sportid)
        {
            _data.DeleteSport(sportid);
            return View("Index");
        }

        public IActionResult AddSportToFavourites(int sportid)
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("userid");
            ViewBag.Sportid = sportid;
            return View();
        }

        [HttpPost]
        public IActionResult AddSportToFavourites(UsersSport userssport)
        {
            _data.AddSportToFavourites(userssport);
            return RedirectToAction("Index", "Sport");
        }

        public IActionResult RemoveSportFromFavourites(int sportid)
        {
            var userid = (int)HttpContext.Session.GetInt32("userid");
            _data.RemoveSportFromFavourites(userid, sportid);
            return RedirectToAction("Index", "Sport");
        }
    }
}
