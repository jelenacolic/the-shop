using System;
using System.Collections.Generic;
using System.Text;
using TheShop.EntityModels.InMemory;

namespace TheShop.Repository.InMemory.Abstractions
{
    public interface IBuyerInMemoryRepository
    {
        Buyer GetById(long id);

        Buyer Insert(Buyer entity);

        void Update(Buyer entity);

        void Delete(long id);

        IList<Buyer> GetAll();

        IList<Buyer> GetAll(Func<Buyer, bool> predicate);
    }
}
