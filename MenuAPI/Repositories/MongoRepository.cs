using Common.Models;
using MenuAPI.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MenuItem
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<T> _collection;
        public MongoRepository(IMongoDbSettings dbSettings)
        {
            _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DataBaseName);

            string tableName = typeof(T).Name.ToLower();
            _collection = _db.GetCollection<T>(tableName);

        }
        
        

        public void DeleteItem(Guid id)
        {
            _collection.DeleteOne(doc => doc.Id == id);
        }

        public List<T> GetAllMenuItems()
        {


            var items = _collection.Find(new BsonDocument()).ToList();

            return items;

        }


        public List<T> GetMenuItemByCategory(string Category)
        {
            var items = _collection.Find(doc => doc.Category == Category).ToList();
            return items;
        }

        public T GetMenuItemById(Guid id)
        {
            var item = _collection.Find(doc => doc.Id == id).FirstOrDefault();
            return item;
        }

        public T InsertMenuItem(T item)
        {
            _collection.InsertOne(item);

            return item;
        }

        public void UpsertItem(T item)
        {

            _collection.ReplaceOne(doc => doc.Id == item.Id, item, new ReplaceOptions() { IsUpsert = true });



        }
    }
}
