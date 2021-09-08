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
        public ActionResult Index()
        {
            return View(_data.GetAllSports());
        }

        // GET: SportController/Details/5
        public ActionResult Details(int sportid)
        {
            return View(_data.GetSportById(sportid));
        }

        // GET: SportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SportController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Sport sport)
        {
            _data.CreateSport(sport);
            return RedirectToAction("Index", "Sport");
        }

        // GET: SportController/Edit/5
        public ActionResult Edit(int sportid)
        {
            return View(_data.GetSportById(sportid));
        }

        // POST: SportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int sportid, Sport sport)
        {
            _data.EditSport(sportid, sport);
            return View("Index");
        }

        // GET: SportController/Delete/5
        public ActionResult Delete(int sportid)
        {
            _data.DeleteSport(sportid);
            return View("Index");
        }

        //// POST: SportController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
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
