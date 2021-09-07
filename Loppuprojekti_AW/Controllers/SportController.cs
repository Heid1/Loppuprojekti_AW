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
        public ActionResult Create(int sportid, string sportname, string description)
        {
            Sport sport = new()
            {
                Sportid = sportid,
                Sportname = sportname,
                Description = description
            };
            _data.CreateSport(sport);
            return RedirectToAction("Index", "Sport");
        }

        // GET: SportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
