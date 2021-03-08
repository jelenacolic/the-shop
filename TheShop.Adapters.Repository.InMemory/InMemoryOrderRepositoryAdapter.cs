using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Adapters.Repository.InMemory
{
    public class InMemoryOrderRepositoryAdapter : IOrderRepositoryAdapter
    {
        #region Private fields
        private IOrderInMemoryRepository _orderRepository;
        private IInventoryInMemoryRepository _inventoryRepository;
        private IArticleInMemoryRepository _articleRepository;
        private IBuyerInMemoryRepository _buyerRepository;
        private ILogger<InMemoryOrderRepositoryAdapter> _logger;
        #endregion

        #region Constructors

        public InMemoryOrderRepositoryAdapter(IOrderInMemoryRepository orderRepository,
            IInventoryInMemoryRepository inventoryRepository, IArticleInMemoryRepository articleRepository,
            IBuyerInMemoryRepository buyerRepository, ILogger<InMemoryOrderRepositoryAdapter> logger)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _articleRepository = articleRepository;
            _buyerRepository = buyerRepository;
            _logger = logger;
        }

        #endregion

        #region Public methods
        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryOrderRepositoryAdapter).FullName}.Delete({id})");

            try
            {
                _orderRepository.Delete(id);
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

        public IList<BusinessModels.Order> GetAll()
        {
            _logger.LogInformation($"{typeof(InMemoryOrderRepositoryAdapter).FullName}.GetAll()");

            try
            {

                var result = new List<BusinessModels.Order>();

                IList<EntityModels.InMemory.Order> orderEntities = _orderRepository.GetAll();


                foreach (var orderEntity in orderEntities)
                {
                    EntityModels.InMemory.Inventory inventoryEntity = _inventoryRepository.GetById(orderEntity.InventoryId);
                    EntityModels.InMemory.Article articleEntity = _articleRepository.GetById(inventoryEntity.ArticleId);
                    EntityModels.InMemory.Buyer buyerEntity = _buyerRepository.GetById(orderEntity.BuyerId);

                    BusinessModels.Order order = CreateBusinessModel(orderEntity, articleEntity, inventoryEntity, buyerEntity);

                    result.Add(order);
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

        public BusinessModels.Order GetById(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryOrderRepositoryAdapter).FullName}.GetById({id})");

            try
            {

                EntityModels.InMemory.Order orderEntity = _orderRepository.GetById(id);
                BusinessModels.Order result = null;

                if (orderEntity != null)
                {
                    EntityModels.InMemory.Inventory inventoryEntity = _inventoryRepository.GetById(orderEntity.InventoryId);
                    EntityModels.InMemory.Article articleEntity = _articleRepository.GetById(inventoryEntity.ArticleId);
                    EntityModels.InMemory.Buyer buyerEntity = _buyerRepository.GetById(orderEntity.BuyerId);

                    result = CreateBusinessModel(orderEntity, articleEntity, inventoryEntity, buyerEntity);

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

        public BusinessModels.Order Insert(BusinessModels.Order order)
        {
            _logger.LogInformation($"{typeof(InMemoryOrderRepositoryAdapter).FullName}.Insert(ArticleId={order.Article.Id}, BuyerId={order.Buyer.Id})");

            try
            {
                EntityModels.InMemory.Order orderEntity = CreateEntityModel(order);

                orderEntity = _orderRepository.Insert(orderEntity);

                order.Id = orderEntity.Id;

                return order;
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

        public void Update(BusinessModels.Order order)
        {
            _logger.LogInformation($"{typeof(InMemoryOrderRepositoryAdapter).FullName}.Update({order.Id})");

            try
            {
                EntityModels.InMemory.Order orderEntity = CreateEntityModel(order);

                _orderRepository.Update(orderEntity);
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

        #region Private methods
        private EntityModels.InMemory.Order CreateEntityModel(BusinessModels.Order order)
        {
            return new EntityModels.InMemory.Order()
            {
                BuyerId = order.Buyer.Id,
                DateSold = DateTime.UtcNow,
                InventoryId = order.Article.InventoryId
            };
        }

        private BusinessModels.Order CreateBusinessModel(EntityModels.InMemory.Order orderEntity, EntityModels.InMemory.Article articleEntity,
            EntityModels.InMemory.Inventory inventoryEntity, EntityModels.InMemory.Buyer buyerEntity)
        {
            return new BusinessModels.Order()
            {
                Id = orderEntity.Id,
                Article = new BusinessModels.Article()
                {
                    Id = articleEntity.Id,
                    EAN = articleEntity.EAN,
                    InventoryId = inventoryEntity.Id,
                    Name = articleEntity.Name,
                    Price = inventoryEntity.Price,
                    // Only 1 can be purchased
                    Quantity = 1
                },
                Buyer = new BusinessModels.Buyer()
                {
                    Id = buyerEntity.Id,
                    Name = buyerEntity.Name
                },
                Date = orderEntity.DateSold
            };
        }

        #endregion
    }
}
