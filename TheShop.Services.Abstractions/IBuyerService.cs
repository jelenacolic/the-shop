using System;
using System.Collections.Generic;
using System.Text;
using TheShop.BusinessModels;

namespace TheShop.Services.Abstractions
{
    public interface IBuyerService
    {
        Buyer AddBuyer(Buyer buyer);

        void UpdateBuyer(Buyer buyer);

        Buyer GetBuyer(long id);

        IList<Buyer> GetAllBuyers();
    }
}
