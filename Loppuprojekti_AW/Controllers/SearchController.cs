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

        public IActionResult GetPostsByCriteria(string criteria)
        {
            if (criteria != null)
            {
                DataAccess da = new DataAccess(_context);
                var postlist = da.GetPostsByCriteria(criteria);
                ViewBag.Posts = postlist;
                return View();

            } else {
                return View();
            }
        }
    }
}
