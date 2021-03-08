using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Adapters.Repository.InMemory
{
    public class InMemoryArticleRepositoryAdapter : IArticleRepositoryAdapter
    {
        #region Private fields
        private IArticleInMemoryRepository _articleRepository;
        private IInventoryInMemoryRepository _inventoryRepository;
        private ILogger<InMemoryArticleRepositoryAdapter> _logger;
        #endregion

        #region Constructors
        public InMemoryArticleRepositoryAdapter(IArticleInMemoryRepository articleRepository, IInventoryInMemoryRepository inventoryRepository, ILogger<InMemoryArticleRepositoryAdapter> logger)
        {
            _articleRepository = articleRepository;
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public bool Delete(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.Delete({id})");

            try
            {
                var articleInventory = _inventoryRepository.GetAll(inv => inv.ArticleId == id);

                if (articleInventory.Any())
                {
                    return false;
                }

                _articleRepository.Delete(id);
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

        public IList<BusinessModels.Article> GetAll()
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.GetAll()");

            try
            {
                var result = new List<BusinessModels.Article>();

                IList<EntityModels.InMemory.Article> articleEntities = _articleRepository.GetAll();

                foreach (var articleEntity in articleEntities)
                {
                    BusinessModels.Article article = CreateBusinessModel(articleEntity);

                    result.Add(article);
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

        public BusinessModels.Article GetByEan(string ean)
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.GetByEan({ean})");

            try
            {
                EntityModels.InMemory.Article articleEntity = _articleRepository.GetByEan(ean);

                BusinessModels.Article article = null;

                if (articleEntity != null)
                {
                    article = CreateBusinessModel(articleEntity);
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

        public BusinessModels.Article GetById(long id)
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.GetById({id})");

            try
            {
                EntityModels.InMemory.Article articleEntity = _articleRepository.GetById(id);
                BusinessModels.Article article = null;

                if (articleEntity != null)
                {
                    article = CreateBusinessModel(articleEntity);
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

        public BusinessModels.Article Insert(BusinessModels.Article article)
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.Insert({article.EAN})");

            try
            {
                EntityModels.InMemory.Article articleEntity = CreateEntityModel(article);

                articleEntity = _articleRepository.Insert(articleEntity);

                article.Id = articleEntity.Id;

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

        public void Update(BusinessModels.Article article)
        {
            _logger.LogInformation($"{typeof(InMemoryArticleRepositoryAdapter).FullName}.Update({article.Id})");

            try
            {
                EntityModels.InMemory.Article articleEntity = CreateEntityModel(article);

                _articleRepository.Update(articleEntity);
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

        private BusinessModels.Article CreateBusinessModel(EntityModels.InMemory.Article articleEntity)
        {
            return new BusinessModels.Article()
            {
                Id = articleEntity.Id,
                EAN = articleEntity.EAN,
                Name = articleEntity.Name
            };
        }

        private EntityModels.InMemory.Article CreateEntityModel(BusinessModels.Article article)
        {
            return new EntityModels.InMemory.Article()
            {
                Id = article.Id,
                EAN = article.EAN,
                Name = article.Name
            };
        }

        #endregion
    }
}
