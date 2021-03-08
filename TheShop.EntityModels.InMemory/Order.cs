using System;

namespace TheShop.EntityModels.InMemory
{
    public class Order : BaseModel
    {
        public long InventoryId { get; set; }

        public long BuyerId { get; set; }

        public DateTime DateSold { get; set; }
    }
}
