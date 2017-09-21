using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutWeb.Models;
using AutWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace AutWeb.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tiles = 
                (await _context.CalendarItems.ToListAsync())
                .Select(x => new Tile(x.ImageUrl, x.Description));

            ViewBag.todayTiles = tiles;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
