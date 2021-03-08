using System;
using System.Collections.Generic;
using System.Text;
using TheShop.EntityModels.InMemory;

namespace TheShop.Repository.InMemory.Abstractions
{
    public interface IOrderInMemoryRepository
    {
        Order GetById(long id);

        Order Insert(Order entity);

        void Update(Order entity);

        void Delete(long id);

        IList<Order> GetAll();

        IList<Order> GetAll(Func<Order, bool> predicate);
    }
}
