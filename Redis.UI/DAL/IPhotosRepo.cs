using Redis.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI.DAL
{
    public interface IPhotosRepo
    {
        public Task<List<Photo>> getAllRedisAsync();
        public Task<List<Photo>> getAllAsync();
    }
}
