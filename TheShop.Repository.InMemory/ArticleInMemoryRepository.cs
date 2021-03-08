using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheShop.EntityModels.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;

namespace TheShop.Repository.InMemory
{
    public class ArticleInMemoryRepository : IArticleInMemoryRepository
    {
        #region Private fields
        private List<Article> _articles;
        private ILogger<ArticleInMemoryRepository> _logger;
        
        private static long Id = 0;
        #endregion

        #region Constructors
        public ArticleInMemoryRepository(ILogger<ArticleInMemoryRepository> logger)
        {
            _logger = logger;
            _articles = new List<Article>();
        }
        #endregion

        #region Public methods

        public void Delete(long id)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.Delete({id})");

            Article existingEntity = null;

            try
            {
                existingEntity = _articles.FirstOrDefault(e => e.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Article with id={id} not found");
                    return;
                }


                _articles.Remove(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }

            
        }

        public IList<Article> GetAll()
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.GetAll()");

            return _articles;
        }

        public IList<Article> GetAll(Func<Article, bool> predicate)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.GetAll() with predicate");

            try
            {
                return _articles.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public Article GetByEan(string ean)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.GetByEan({ean})");

            try
            {
                return _articles.FirstOrDefault(e => e.EAN.Equals(ean));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public Article GetById(long id)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.GetById({id})");

            try
            {
                return _articles.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
            
        }

        public Article Insert(Article entity)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.Insert({entity.EAN})");

            try
            {
                entity.Id = ++Id;
                _articles.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new LoggedException("Logged exception", ex);
            }
        }

        public void Update(Article entity)
        {
            _logger.LogInformation($"{typeof(ArticleInMemoryRepository).FullName}.Update({entity.Id})");

            try
            {
                var existingEntity = _articles.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogInformation($"Article with id={entity.Id} not found");
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
