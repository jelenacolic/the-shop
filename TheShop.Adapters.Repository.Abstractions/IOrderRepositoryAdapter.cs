using System;
using System.Collections.Generic;
using System.Text;
using TheShop.BusinessModels;

namespace TheShop.Adapters.Repository.Abstractions
{
    public interface IOrderRepositoryAdapter 
    {
        Order GetById(long id);

        Order Insert(Order buyer);

        void Update(Order buyer);

        void Delete(long id);

        IList<Order> GetAll();
    }
}
