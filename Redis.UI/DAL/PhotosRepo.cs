using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.UI.DAL
{
    public class PhotosRepo: IPhotosRepo
    {
        private readonly IDistributedCache _distributedCache;
        public PhotosRepo(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<List<Photo>> getAllRedisAsync()
        {
            string cachedPhoto = await _distributedCache.GetStringAsync("Photo");
            List<Photo> photos;
            if (string.IsNullOrEmpty(cachedPhoto))
            {
                photos = await getAllAsync();
                if (photos is null)
                {
                    return photos;
                }
                await _distributedCache.SetStringAsync("Photo", JsonConvert.SerializeObject(photos));
                return photos;
            }
            photos = JsonConvert.DeserializeObject<List<Photo>>(cachedPhoto);
            return photos;
        }
        public async Task<List<Photo>> getAllAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string photosUrl = "https://jsonplaceholder.typicode.com/photos";

                HttpResponseMessage response = await client.GetAsync(photosUrl);
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                List<Photo> photos = JsonConvert.DeserializeObject<List<Photo>>(jsonString);
                return photos;
            }
        }
    }
}
