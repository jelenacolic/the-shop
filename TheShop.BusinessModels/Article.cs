namespace TheShop.BusinessModels
{
    public class Article : BaseModel
    {
        public string Name { get; set; }

        public string EAN { get; set; }

        public long InventoryId { get; set; }

        public double Price { get; set; }

        public long Quantity { get; set; }
    }
}
