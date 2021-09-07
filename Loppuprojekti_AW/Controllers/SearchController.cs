using Loppuprojekti_AW.Models;
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

        public IActionResult GetPostsByCriteria(string criteria)
        {
            if (criteria != null)
            {
                DataAccess da = new DataAccess(_context);
                var postlist = da.GetPostsByCriteria(criteria);
                ViewBag.Posts = postlist;
                return View();

            } else {
                //string error = "Anna hakusana";
                //ViewBag.Alert = error; //Tässä viewbagiin menee error viesti joka tulostetaan viewissä bootstrap alertissa
                return View();
            }
        }
    }
}
