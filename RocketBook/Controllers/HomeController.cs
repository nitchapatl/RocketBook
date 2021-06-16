using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RocketBook.Models.ViewModels;
using RocketBook.data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace RocketBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        public IActionResult Index(int? id)
        {


            if (id == null)
            {
                HomeVM homeVM = new HomeVM()
                {
                    Books = _db.Book.Include(u => u.Category),
                    Categories = _db.Category,
                    DisplayCategory = "All"
                };
                return View(homeVM);
            }
            else
            {
                HomeVM homeVM = new HomeVM()
                {
                    Books = _db.Book.Where(x => x.CategoryId == id).Include(u => u.Category),
                    Categories = _db.Category,
                    DisplayCategory = _db.Category.FirstOrDefault(x => x.Id == id).Name
                };
                return View(homeVM);
            }

        }


        public IActionResult Details(int id)
        {
            ViewData["Reffer"] = Request.Headers["Referer"].ToString(); 

            DetailsVM DetailsVM = new DetailsVM()
            {
                Book = _db.Book.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault()
            };


            return View(DetailsVM);
        }

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
