using System.Collections.Generic;

namespace TheShop.BusinessModels
{
    public class Supplier : BaseModel
    {
        public Supplier()
        {
            Inventory = new Dictionary<long, Article>();
        }

        public string Name { get; set; }

        public Dictionary<long, Article> Inventory { get; set; }
    }
}
