using Microsoft.Extensions.Logging;
using System;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.BusinessModels;
using TheShop.Exceptions;
using TheShop.Services.Abstractions;

namespace TheShop.Services
{
    public class ShopService : IShopService
    {
        #region Private fields
        private IOrderRepositoryAdapter _orderAdapter;
		private ILogger<ShopService> _logger;
        #endregion

        #region Constructors
        public ShopService(ILogger<ShopService> logger, IOrderRepositoryAdapter orderAdapter)
		{
            _orderAdapter = orderAdapter;
			_logger = logger;
		}
        #endregion

        #region Public methods
        public void SellArticle(Article article, Buyer buyer)
        {
            _logger.LogInformation($"{typeof(ShopService).FullName}.SellArticle(articleId={article.Id}, buyerId={buyer.Id})");

            try
            {
                var order = new Order()
                {
                    Article = article,
                    Buyer = buyer,
                    Date = DateTime.UtcNow
                };

                _orderAdapter.Insert(order);
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
