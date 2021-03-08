using System;

namespace TheShop.BusinessModels
{
    public class Order : BaseModel
    {
        public Article Article { get; set; }

        public Buyer Buyer { get; set; }

        public DateTime Date { get; set; }
    }
}
