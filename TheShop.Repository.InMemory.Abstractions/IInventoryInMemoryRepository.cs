using System;
using System.Collections.Generic;
using TheShop.EntityModels.InMemory;

namespace TheShop.Repository.InMemory.Abstractions
{
    public interface IInventoryInMemoryRepository
    {
        Inventory GetById(long id);

        Inventory Insert(Inventory entity);

        void Update(Inventory entity);

        void Delete(long id);

        IList<Inventory> GetAll();

        IList<Inventory> GetAll(Func<Inventory, bool> predicate);
    }
}
