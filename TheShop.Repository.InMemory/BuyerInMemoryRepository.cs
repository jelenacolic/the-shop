using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.EntityModels.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Repository.InMemory
{
    public class BuyerInMemoryRepository : IBuyerInMemoryRepository
    {
        #region Private fields
        private List<Buyer> _buyers;
        private ILogger<BuyerInMemoryRepository> _logger;

        private static long Id = 0;
        #endregion

        #region Constructors
        public BuyerInMemoryRepository(ILogger<BuyerInMemoryRepository> logger)
        {
            _logger = logger;
            _buyers = new List<Buyer>();
        }
        #endregion

        #region Public methods
        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.Delete({id})");
            Buyer existingEntity = null;

            try
            {
                existingEntity = _buyers.FirstOrDefault(e => e.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Buyer with id={id} not found");
                    return;
                }

                _buyers.Remove(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
 
        }

        public IList<Buyer> GetAll()
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.GetAll()");

            return _buyers;
        }

        public IList<Buyer> GetAll(Func<Buyer, bool> predicate)
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.GetAll() with predicate");

            try
            {
                return _buyers.Where(predicate).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public Buyer GetById(long id)
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.GetById({id})");

            try
            {
                return _buyers.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }

            
        }

        public Buyer Insert(Buyer entity)
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.Insert({entity.Name})");

            try
            {
                entity.Id = ++Id;
                _buyers.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public void Update(Buyer entity)
        {
            _logger.LogInformation($"{typeof(BuyerInMemoryRepository).FullName}.Update({entity.Id})");

            try
            {
                var existingEntity = _buyers.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Buyer with id={entity.Id} not found");
                    return;
                }

                existingEntity.Name = entity.Name;
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
