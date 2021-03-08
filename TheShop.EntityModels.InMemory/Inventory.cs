namespace TheShop.EntityModels.InMemory
{
    public class Inventory : BaseModel
    {
        public long SupplierId { get; set; }

        public long ArticleId { get; set; }

        public long Quantity { get; set; }

        public double Price { get; set; }
    }
}
