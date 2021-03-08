using System.Collections.Generic;
using TheShop.BusinessModels;

namespace TheShop.Adapters.Repository.Abstractions
{
    public interface IBuyerRepositoryAdapter
    {
        Buyer GetById(long id);

        Buyer Insert(Buyer buyer);

        void Update(Buyer buyer);

        bool Delete(long id);

        IList<Buyer> GetAll();

    }
}
