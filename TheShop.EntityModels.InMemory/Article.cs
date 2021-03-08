namespace TheShop.EntityModels.InMemory
{
	public class Article : BaseModel
	{
		public string Name { get; set; }

		public string EAN { get; set; }
    }
}
