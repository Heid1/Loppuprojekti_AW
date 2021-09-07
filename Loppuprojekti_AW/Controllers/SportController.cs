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
        //private readonly MoveoContext _context;

        //public SportController(MoveoContext context)
        //{
        //    _context = context;
        //}

        // GET: SportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SportController/Details/5
        public ActionResult Details(int sportid)
        {
            Sport sport = DataAccess.GetSportById(sportid);
            return View(sport);
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
            DataAccess.CreateSport(sport);

            return RedirectToAction("Index", "Home");
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
