using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.BusinessModels;
using TheShop.Exceptions;
using TheShop.Services.Abstractions;

namespace TheShop.Services
{
    public class SupplierService : ISupplierService
    {
        #region Private fields
        private ISupplierRepositoryAdapter _supplierAdapter;
        private ILogger<SupplierService> _logger;
        #endregion

        #region Constructors
        public SupplierService(ILogger<SupplierService> logger, ISupplierRepositoryAdapter supplierAdapter) 
        {
            _logger = logger;
            _supplierAdapter = supplierAdapter;
        }
        #endregion

        #region Public methods
        public void AddSupplier(Supplier supplier)
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.AddSupplier({supplier.Name})");

            try
            {
                _supplierAdapter.Insert(supplier);
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void AddArticleToSupplierInventory(Supplier supplier, Article article)
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.AddArticleToSupplierInventory(supplierId={supplier.Id}, articleId={article.Id})");

            try
            {
                _supplierAdapter.AddArticleToSupplierInventory(supplier.Id, article);
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public Article OrderArticle(string ean, double maxExpectedPrice)
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.OrderArticle(ean={ean}, maxExpectedPrice={maxExpectedPrice})");

            try
            {
                var suppliers = _supplierAdapter.GetSuppliersWithArticleInInventory(ean, maxExpectedPrice);

                if (!suppliers.Any())
                {
                    return null;
                }

                var articles = suppliers.SelectMany(sup => sup.Inventory.Values);

                if (articles.Count() == 0)
                {
                    return null;
                }

                var article = articles.OrderBy(art => art.Price).FirstOrDefault();

                if (article != null)
                {
                    _supplierAdapter.DecreaseArticleQuantity(article.InventoryId);
                }

                return article;
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void RemoveSupplier(long id)
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.RemoveSupplier({id})");

            try
            {
                _supplierAdapter.Delete(id);
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void ReturnArticleToSupplier(Article article)
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.ReturnArticleToSupplier({article.Id})");

            try
            {
                _supplierAdapter.IncreaseArticleQuantity(article.InventoryId);
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }

        }

        public IList<Supplier> GetAllSuppliers()
        {
            _logger.LogInformation($"{typeof(SupplierService).FullName}.GetAllSuppliers()");

            try
            {
                return _supplierAdapter.GetAll();
            }
            catch (LoggedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }
        #endregion
    }
}
