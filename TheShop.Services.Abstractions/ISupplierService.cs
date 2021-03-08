using System.Collections.Generic;
using TheShop.BusinessModels;

namespace TheShop.Services.Abstractions
{
    public interface ISupplierService
    {
        void AddSupplier(Supplier supplier);

        void RemoveSupplier(long id);

        Article OrderArticle(string ean, double maxExpectedPrice);

        void AddArticleToSupplierInventory(Supplier supplier, Article article);

        void ReturnArticleToSupplier(Article article);

        IList<Supplier> GetAllSuppliers();
    }
}
