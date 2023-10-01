using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Redis.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.UI.DAL
{
    public class PhotosMongo:IPhotosMongo
    {
        private readonly IMongoCollection<Photo> mongoCollection;
        public PhotosMongo(string mongoConnectionString, string dbName, string collectionName)
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(dbName);
            mongoCollection = database.GetCollection<Photo>(collectionName);
        }

        public async Task<List<Photo>> GetPhotos()
        {
            var data = await mongoCollection.FindAsync(book => true);
            var photos = await data.ToListAsync();
            return photos;
        }
        public Photo GetById(int Id)
        {
            return mongoCollection.Find<Photo>(m => m.id == Id).FirstOrDefault();
        }
        public Photo Create(Photo model)
        {
            mongoCollection.InsertOne(model);
            return model;
        }
        public List<Photo> CreateAll(List<Photo> model)
        {
            mongoCollection.InsertMany(model);
            return model;
        }
        public void Update(int Id, Photo model)
        {
            mongoCollection.ReplaceOne(m => m.id == Id, model);
        }
        public void Delete(int Id)
        {
            mongoCollection.DeleteOne(m => m.id == Id);
        }
    }
}
