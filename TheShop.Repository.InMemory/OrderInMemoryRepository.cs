using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.EntityModels.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Repository.InMemory
{
    public class OrderInMemoryRepository : IOrderInMemoryRepository
    {
        #region Private fields
        private List<Order> _orders { get; set; }
        private ILogger<OrderInMemoryRepository> _logger;

        private static long Id = 0;
        #endregion

        #region Constructors
        public OrderInMemoryRepository(ILogger<OrderInMemoryRepository> logger)
        {
            _logger = logger;
            _orders = new List<Order>();
        }
        #endregion

        #region Public methods
        public Order GetById(long id)
        {
            _logger.LogInformation($"{typeof(OrderInMemoryRepository).FullName}.GetById({id})");

            try
            {
                return _orders.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public Order Insert(Order entity)
        {
            _logger.LogInformation($"{typeof(OrderInMemoryRepository).FullName}.Insert(BuyerId={entity.BuyerId}, InventoryId={entity.InventoryId})");

            try
            {
                entity.Id = ++Id;
                _orders.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void Update(Order entity)
        {
            _logger.LogInformation($"{typeof(OrderInMemoryRepository).FullName}.Update({entity.Id})");

            try
            {
                var existingEntity = _orders.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Order with id={entity.Id} not found");

                    return;
                }

                existingEntity.DateSold = entity.DateSold;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(OrderInMemoryRepository).FullName}.Delete({id})");

            Order existingEntity = null;

            try
            {
                existingEntity = _orders.FirstOrDefault(e => e.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Order with id={id} not found");
                    return;
                }

                _orders.Remove(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public IList<Order> GetAll()
        {
            _logger.LogInformation($"{typeof(OrderInMemoryRepository).FullName}.GetAll()");

            return _orders;
        }

        public IList<Order> GetAll(Func<Order, bool> predicate)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.GetAll() with predicate");

            try
            {
                return _orders.Where(predicate).ToList();
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
