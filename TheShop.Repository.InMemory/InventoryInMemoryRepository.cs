using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheShop.EntityModels.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Repository.InMemory
{
    public class InventoryInMemoryRepository : IInventoryInMemoryRepository
    {
        #region Private fields
        private List<Inventory> _inventories { get; set; }
        private ILogger<InventoryInMemoryRepository> _logger;

        private static long Id = 0;
        #endregion

        #region Constructors

        public InventoryInMemoryRepository(ILogger<InventoryInMemoryRepository> logger)
        {
            _logger = logger;
            _inventories = new List<Inventory>();
        }
        #endregion

        #region Public methods

        public Inventory GetById(long id)
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.GetById({id})");

            try
            {
                return _inventories.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public Inventory Insert(Inventory entity)
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.Insert(supplierId={entity.SupplierId}, articleId={entity.ArticleId})");

            try
            {
                entity.Id = ++Id;
                _inventories.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void Update(Inventory entity)
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.Update({entity.Id})");

            try
            {
                var existingEntity = _inventories.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Inventory with id={entity.Id} not found");
                    return;
                }


                existingEntity.Price = entity.Price;
                existingEntity.Quantity = entity.Quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.Delete({id})");

            Inventory existingEntity = null;

            try
            {
                existingEntity = _inventories.FirstOrDefault(e => e.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Inventory with id={id} not found");
                    return;
                }

                _inventories.Remove(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public IList<Inventory> GetAll()
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.GetAll()");

            return _inventories;
        }

        public IList<Inventory> GetAll(Func<Inventory, bool> predicate)
        {
            _logger.LogInformation($"{typeof(InventoryInMemoryRepository).FullName}.GetAll() with predicate");

            try
            {
                return _inventories.Where(predicate).ToList();
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
