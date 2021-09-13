using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Loppuprojekti_AW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoveoContext _context;

        public HomeController(ILogger<HomeController> logger, MoveoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
           
            DataAccess da = new DataAccess(_context);
            var prevalencelist = da.GetAllSports();
            ViewBag.CommonPosts = prevalencelist;
            return View();
        }

        public IActionResult Index2()
        {
            DataAccess da = new DataAccess(_context);

            var posts = da.GetAllPosts();
            string posts1 = JsonConvert.SerializeObject(posts);

            ViewBag.Posts = posts1;

            return View();
        }

        public IActionResult Azuremap()
        {
            return View();
        }

        // NÄIHIN EN KEKSINYT HELPPOA RATKAISUA
        public IActionResult SendFeedback()
        {
            return View();
        }
        //public IActionResult SendFeedback(string text)
        //{

        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
