using TheShop.BusinessModels;

namespace TheShop.Services.Abstractions
{
    public interface IShopService
    {
        void SellArticle(Article article, Buyer buyer);
    }
}
