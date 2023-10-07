using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Redis.UI.DAL;
using Redis.UI.Entities;
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
        private readonly IPhotosMongo _mongoRepo;
        private readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, IPhotosRepo photosRepo, IPhotosMongo mongoRepo, IDistributedCache distributedCache)
        {
            _logger = logger;
            _photosRepo = photosRepo;
            _mongoRepo = mongoRepo;
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> GetFromLink()
        {
            List<Photo> photos;
            string cachedPhoto = await _distributedCache.GetStringAsync("Photo");
            if (string.IsNullOrEmpty(cachedPhoto))
            {
                photos = await _photosRepo.getAllAsync();
                if (photos is null) return View(photos);
                await _distributedCache.SetStringAsync("Photo", JsonConvert.SerializeObject(photos));
                return View(photos);
            }
            photos = JsonConvert.DeserializeObject<List<Photo>>(cachedPhoto);
            return View(photos);
        }
        public async Task<IActionResult> GetFromMongo()
        {
            List<Photo> photos;
            string cachedPhoto = await _distributedCache.GetStringAsync("Photo");
            if (string.IsNullOrEmpty(cachedPhoto))
            {
                photos = await _mongoRepo.GetPhotos();
                if(photos is null) return View(photos);
                await _distributedCache.SetStringAsync("Photo", JsonConvert.SerializeObject(photos));
                return View(photos);
            }
            photos = JsonConvert.DeserializeObject<List<Photo>>(cachedPhoto);
            return View(photos);
        }
        [HttpGet]
        public async Task<IActionResult> AddMongo()
        {
            return View(new Photo());
        }
        [HttpPost]
        public async Task<IActionResult> AddMongo(Photo photo)
        {
            await _distributedCache.RemoveAsync("Photo");
            _mongoRepo.Create(photo);
            
            return RedirectToAction("GetFromMongo");
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
