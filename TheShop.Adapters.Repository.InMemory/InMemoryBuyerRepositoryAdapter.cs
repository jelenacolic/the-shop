using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Adapters.Repository.InMemory
{
    public class InMemoryBuyerRepositoryAdapter : IBuyerRepositoryAdapter
    {
        #region Private fields
        private IBuyerInMemoryRepository _buyerRepository;
        private IOrderInMemoryRepository _orderRepository;
        private ILogger<InMemoryBuyerRepositoryAdapter> _logger;
        #endregion

        #region Constructors
        public InMemoryBuyerRepositoryAdapter(IBuyerInMemoryRepository buyerRepository, IOrderInMemoryRepository orderRepository, ILogger<InMemoryBuyerRepositoryAdapter> logger)
        {
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public bool Delete(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryBuyerRepositoryAdapter).FullName}.Delete({id})");

            try
            {
                var buyerOrders = _orderRepository.GetAll(o => o.BuyerId == id);

                if (buyerOrders.Any())
                {
                    return false;
                }
                
                _buyerRepository.Delete(id);

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

        public IList<BusinessModels.Buyer> GetAll()
        {
            _logger.LogInformation($"{typeof(InMemoryBuyerRepositoryAdapter).FullName}.GetAll()");

            try
            {
                var result = new List<BusinessModels.Buyer>();

                IList<EntityModels.InMemory.Buyer> buyerEntities = _buyerRepository.GetAll();

                foreach (var buyerEntity in buyerEntities)
                {
                    BusinessModels.Buyer buyer = CreateBusinessModel(buyerEntity);

                    result.Add(buyer);
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

        public BusinessModels.Buyer GetById(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryBuyerRepositoryAdapter).FullName}.GetById({id})");

            try
            {
                EntityModels.InMemory.Buyer buyerEntity = _buyerRepository.GetById(id);

                BusinessModels.Buyer buyer = null;
                
                if (buyerEntity != null)
                {
                    buyer = CreateBusinessModel(buyerEntity);
                }
                
                return buyer;
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

        public BusinessModels.Buyer Insert(BusinessModels.Buyer buyer)
        {
            _logger.LogInformation($"{typeof(InMemoryBuyerRepositoryAdapter).FullName}.Insert({buyer.Name})");

            try
            {
                EntityModels.InMemory.Buyer buyerEntity = CreateEntityModel(buyer);

                buyerEntity = _buyerRepository.Insert(buyerEntity);

                buyer.Id = buyerEntity.Id;

                return buyer;
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

        public void Update(BusinessModels.Buyer buyer)
        {
            _logger.LogInformation($"{typeof(InMemoryBuyerRepositoryAdapter).FullName}.Update({buyer.Id})");

            try
            {

                EntityModels.InMemory.Buyer buyerEntity = CreateEntityModel(buyer);

                _buyerRepository.Update(buyerEntity);
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
        private BusinessModels.Buyer CreateBusinessModel(EntityModels.InMemory.Buyer buyerEntity)
        {
            return new BusinessModels.Buyer()
            {
                Id = buyerEntity.Id,
                Name = buyerEntity.Name
            };
        }

        private EntityModels.InMemory.Buyer CreateEntityModel(BusinessModels.Buyer buyer)
        {
            return new EntityModels.InMemory.Buyer()
            {
                Id = buyer.Id,
                Name = buyer.Name
            };
        }

        #endregion
    }
}
