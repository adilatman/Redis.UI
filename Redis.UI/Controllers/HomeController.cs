using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Redis.UI.DAL;
using Redis.UI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotosRepo _photosRepo;

        public HomeController(ILogger<HomeController> logger, IPhotosRepo photosRepo)
        {
            _logger = logger;
            _photosRepo = photosRepo;
        }

        public async Task<IActionResult> Index()
        {
            var photos = await _photosRepo.getAllRedisAsync();
            return View(photos);
        }

        public async Task<IActionResult> Index2()
        {
            var photos = await _photosRepo.getAllAsync();
            return View(photos);
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
