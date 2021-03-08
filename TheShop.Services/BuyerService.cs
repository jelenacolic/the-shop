using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.BusinessModels;
using TheShop.Exceptions;
using TheShop.Services.Abstractions;

namespace TheShop.Services
{
    public class BuyerService : IBuyerService
    {
        #region Private fields
        private IBuyerRepositoryAdapter _buyerAdapter;
        private ILogger<BuyerService> _logger;
        #endregion

        #region Constructors
        public BuyerService(IBuyerRepositoryAdapter buyerAdapter, ILogger<BuyerService> logger)
        {
            _buyerAdapter = buyerAdapter;
            _logger = logger;

        }
        #endregion

        #region Public methods
        public Buyer AddBuyer(Buyer buyer)
        {
            _logger.LogInformation($"{typeof(BuyerService).FullName}.AddBuyer({buyer.Name})");

            try
            {
                return _buyerAdapter.Insert(buyer);

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

        public IList<Buyer> GetAllBuyers()
        {
            _logger.LogInformation($"{typeof(BuyerService).FullName}.GetAllBuyers()");

            try
            {
                return _buyerAdapter.GetAll();
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

        public Buyer GetBuyer(long id)
        {
            _logger.LogInformation($"{typeof(BuyerService).FullName}.GetBuyer({id})");

            try
            {
                return _buyerAdapter.GetById(id);
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

        public void UpdateBuyer(Buyer buyer)
        {
            _logger.LogInformation($"{typeof(BuyerService).FullName}.UpdateBuyer({buyer.Id})");

            try
            {
                _buyerAdapter.Update(buyer);
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
