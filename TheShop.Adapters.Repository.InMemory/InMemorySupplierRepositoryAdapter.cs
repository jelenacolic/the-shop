using System;
using System.Linq;
using System.Collections.Generic;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.Repository.InMemory.Abstractions;
using Microsoft.Extensions.Logging;
using TheShop.Exceptions;

namespace TheShop.Adapters.Repository.InMemory
{
    public class InMemorySupplierRepositoryAdapter : ISupplierRepositoryAdapter
    {
        #region Private fields
        private ISupplierInMemoryRepository _supplierRepository;
        private IArticleInMemoryRepository _articleRepository;
        private IInventoryInMemoryRepository _inventoryRepository;
        private ILogger<InMemorySupplierRepositoryAdapter> _logger;
        #endregion

        #region Constructors
        public InMemorySupplierRepositoryAdapter(ISupplierInMemoryRepository supplierRepository,
            IArticleInMemoryRepository articleRepository,
            IInventoryInMemoryRepository inventoryRepository, ILogger<InMemorySupplierRepositoryAdapter> logger)
        {
            _supplierRepository = supplierRepository;
            _articleRepository = articleRepository;
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public void AddArticleToSupplierInventory(long supplierId, BusinessModels.Article article)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.AddArticleToSupplierInventory(supplierId={supplierId}, articleId={article.Id})");

            try
            {
                var inventoryEntity = new EntityModels.InMemory.Inventory()
                {
                    ArticleId = article.Id,
                    Price = article.Price,
                    Quantity = article.Quantity,
                    SupplierId = supplierId
                };

                _inventoryRepository.Insert(inventoryEntity);
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

        public void DecreaseArticleQuantity(long inventoryId)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.DecreaseArticleQuantity(inventoryId={inventoryId})");

            try
            {
                EntityModels.InMemory.Inventory inventoryEntity = _inventoryRepository.GetById(inventoryId);

                inventoryEntity.Quantity--;

                _inventoryRepository.Update(inventoryEntity);
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

        public void IncreaseArticleQuantity(long inventoryId)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.IncreaseArticleQuantity(inventoryId={inventoryId})");

            try
            {
                EntityModels.InMemory.Inventory inventoryEntity = _inventoryRepository.GetById(inventoryId);
                inventoryEntity.Quantity++;
                _inventoryRepository.Update(inventoryEntity);
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

        public bool Delete(long id)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.Delete({id})");

            try
            {
                var supplierInventory = _inventoryRepository.GetAll(inv => inv.SupplierId == id);

                if (!supplierInventory.Any())
                {
                    return false;
                }

                _supplierRepository.Delete(id);

                return true;
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

        public IList<BusinessModels.Supplier> GetAll()
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.GetAll()");

            try
            {
                IList<EntityModels.InMemory.Supplier> suppliers = _supplierRepository.GetAll();
                IList<EntityModels.InMemory.Inventory> inventories = _inventoryRepository.GetAll(inv => inv.Quantity > 0);
                IList<EntityModels.InMemory.Article> articles = _articleRepository.GetAll(art => inventories.Any(inv => inv.ArticleId == art.Id));

                var result = new List<BusinessModels.Supplier>();

                foreach (var supplierEntity in suppliers)
                {
                    var supplierInventory = inventories.Where(inv => inv.SupplierId == supplierEntity.Id).ToList();

                    BusinessModels.Supplier supplierBusiness = CreateBusinessModel(supplierEntity, articles, supplierInventory);

                    result.Add(supplierBusiness);

                }

                return result;
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

        public IList<BusinessModels.Supplier> GetSuppliersWithArticleInInventory(string ean, double maxExpectedPrice)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.GetSuppliersWithArticleInInventory(ean={ean}, maxExpectedPrice={maxExpectedPrice})");

            try
            { 
                EntityModels.InMemory.Article articleEntity = _articleRepository.GetByEan(ean);

                IList<BusinessModels.Supplier> result = new List<BusinessModels.Supplier>();

                if (articleEntity == null)
                {
                    return result;
                }

                IList<EntityModels.InMemory.Inventory> inventoryItems = _inventoryRepository.GetAll(inv => inv.ArticleId == articleEntity.Id && inv.Quantity > 0 && inv.Price <= maxExpectedPrice);

                if (inventoryItems.Count == 0)
                {
                    return result;
                }

                IList<EntityModels.InMemory.Supplier> suppliers = _supplierRepository.GetAll(sup => inventoryItems.Any(inv => inv.SupplierId == sup.Id));
                
                IList<EntityModels.InMemory.Article> articleEntities = new List<EntityModels.InMemory.Article>()
                {
                    articleEntity
                };

                foreach (var supplier in suppliers)
                {
                    var supplierInventoryItems = inventoryItems.Where(inv => inv.ArticleId == articleEntity.Id && inv.SupplierId == supplier.Id).ToList();

                    BusinessModels.Supplier supplierBusiness = CreateBusinessModel(supplier, articleEntities, supplierInventoryItems);

                    result.Add(supplierBusiness);

                }

                return result;
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

        public BusinessModels.Supplier GetById(long id)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.GetById({id})");

            try
            {
                EntityModels.InMemory.Supplier supplierEntity = _supplierRepository.GetById(id);
                BusinessModels.Supplier result = null;

                if (supplierEntity != null)
                {
                    IList<EntityModels.InMemory.Inventory> inventory = _inventoryRepository.GetAll(inv => inv.SupplierId == id);
                    IList<EntityModels.InMemory.Article> articles = _articleRepository.GetAll(art => inventory.Any(inv => inv.ArticleId == art.Id));

                    result = CreateBusinessModel(supplierEntity, articles, inventory);
                }

                return result;
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

        public BusinessModels.Supplier Insert(BusinessModels.Supplier supplier)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.Insert({supplier.Name})");

            try
            {
                EntityModels.InMemory.Supplier supplierEntity = CreateEntityModel(supplier);

                supplierEntity = _supplierRepository.Insert(supplierEntity);

                supplier.Id = supplierEntity.Id;

                return supplier;
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

        public void Update(BusinessModels.Supplier supplier)
        {
            _logger.LogInformation($"{typeof(InMemorySupplierRepositoryAdapter).FullName}.Update({supplier.Id})");

            try
            {
                EntityModels.InMemory.Supplier supplierEntity = CreateEntityModel(supplier);

                _supplierRepository.Update(supplierEntity);
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


        private EntityModels.InMemory.Supplier CreateEntityModel(BusinessModels.Supplier supplier)
        {
            return new EntityModels.InMemory.Supplier()
            {
                Id = supplier.Id,
                Name = supplier.Name
            };
        }
        #endregion

        #region Private methods
        private BusinessModels.Supplier CreateBusinessModel(EntityModels.InMemory.Supplier supplierEntity, 
            IList<EntityModels.InMemory.Article> articleEntities, IList<EntityModels.InMemory.Inventory> inventoryEntities)
        {
            var result = new BusinessModels.Supplier()
            {
                Id = supplierEntity.Id,
                Name = supplierEntity.Name
            };

            foreach (var item in inventoryEntities)
            {
                EntityModels.InMemory.Article articleEntity = articleEntities.First(art => art.Id == item.ArticleId);

                var articleBusiness = new BusinessModels.Article()
                {
                    Id = articleEntity.Id,
                    EAN = articleEntity.EAN,
                    Name = articleEntity.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    InventoryId = item.Id
                };

                result.Inventory.Add(articleBusiness.Id, articleBusiness);
            }

            return result;
        }
        #endregion
    }
}
