using System;
using System.Collections.Generic;
using TheShop.BusinessModels;

namespace TheShop.Adapters.Repository.Abstractions
{
    public interface ISupplierRepositoryAdapter 
    {
        Supplier GetById(long id);

        Supplier Insert(Supplier supplier);

        void Update(Supplier supplier);

        bool Delete(long id);

        IList<Supplier> GetAll();

        void AddArticleToSupplierInventory(long supplierId, Article article);

        IList<Supplier> GetSuppliersWithArticleInInventory(string ean, double maxExpectedPrice);

        void DecreaseArticleQuantity(long inventoryId);

        void IncreaseArticleQuantity(long inventoryId);
    }
}
