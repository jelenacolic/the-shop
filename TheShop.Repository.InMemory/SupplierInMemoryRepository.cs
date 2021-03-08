using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.EntityModels.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Repository.InMemory
{
    public class SupplierInMemoryRepository : ISupplierInMemoryRepository
    {
        #region Private fields
        private List<Supplier> _suppliers { get; set; }
        private ILogger<SupplierInMemoryRepository> _logger;

        private static long Id = 0;
        #endregion

        #region Constructors
        public SupplierInMemoryRepository(ILogger<SupplierInMemoryRepository> logger)
        {
            _logger = logger;
            _suppliers = new List<Supplier>();
        }
        #endregion

        #region Public methods
        public Supplier GetById(long id)
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.GetById({id})");

            try
            {
                return _suppliers.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public Supplier Insert(Supplier entity)
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.Insert({entity.Name})");

            try
            {
                entity.Id = ++Id;
                _suppliers.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public void Update(Supplier entity)
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.Update({entity.Id})");

            try
            {
                var existingEntity = _suppliers.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Supplier with id={entity.Id} not found");
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

        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.Delete({id})");

            Supplier existingEntity = null;

            try
            {
                existingEntity = _suppliers.FirstOrDefault(e => e.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Supplier with id={id} not found");

                    return;
                }

                _suppliers.Remove(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }

        }

        public IList<Supplier> GetAll()
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.GetAll()");

            return _suppliers;
        }

        public IList<Supplier> GetAll(Func<Supplier, bool> predicate)
        {
            _logger.LogInformation($"{typeof(SupplierInMemoryRepository).FullName}.GetAll() with predicate");

            try
            {
                return _suppliers.Where(predicate).ToList();
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
