using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Repositories
{
    public interface IMongoRepository<T> where T: MongoDocument
    {
        List<T> GetAllMenuItems();
        T InsertMenuItem(T item);
        T GetMenuItemById(Guid id);
        List<T> GetMenuItemByCategory(String  Category);

        void UpsertItem(T item);

        void DeleteItem(Guid id);

    }
}
