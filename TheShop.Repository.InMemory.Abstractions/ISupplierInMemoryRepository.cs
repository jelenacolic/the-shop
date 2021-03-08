using System;
using System.Collections.Generic;
using TheShop.EntityModels.InMemory;

namespace TheShop.Repository.InMemory.Abstractions
{
    public interface ISupplierInMemoryRepository
    {
        Supplier GetById(long id);

        Supplier Insert(Supplier entity);

        void Update(Supplier entity);

        void Delete(long id);

        IList<Supplier> GetAll();

        IList<Supplier> GetAll(Func<Supplier, bool> predicate);
    }
}
