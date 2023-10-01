using Redis.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI.DAL
{
    public interface IPhotosMongo
    {
        public Task<List<Photo>> GetPhotos();
        public Photo GetById(int Id);
        public Photo Create(Photo model);
        public List<Photo> CreateAll(List<Photo> model);
        public void Update(int Id, Photo model);
        public void Delete(int Id);
    }
}
