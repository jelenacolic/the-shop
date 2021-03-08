using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.BusinessModels;
using TheShop.Exceptions;
using TheShop.Services.Abstractions;

namespace TheShop.Services
{
    public class ArticleService : IArticleService
    {
        #region Private fields
        private IArticleRepositoryAdapter _articleAdapter;
        private ILogger<ArticleService> _logger;
        #endregion

        #region Constructors

        public ArticleService(IArticleRepositoryAdapter articleAdapter, ILogger<ArticleService> logger)
        {
            _articleAdapter = articleAdapter;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public Article AddArticle(Article article)
        {
            _logger.LogInformation($"{typeof(ArticleService).FullName}.AddArticle({article.Id})");

            try
            {
                return _articleAdapter.Insert(article);
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

        public IList<Article> GetAllArticles()
        {
            _logger.LogInformation($"{typeof(ArticleService).FullName}.GetAllArticles()");

            try
            {
                return _articleAdapter.GetAll();
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

        public Article GetArticle(string ean)
        {
            _logger.LogInformation($"{typeof(ArticleService).FullName}.GetArticle({ean})");

            try
            {
                return _articleAdapter.GetByEan(ean);
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

        public Article GetArticle(long id)
        {
            _logger.LogInformation($"{typeof(ArticleService).FullName}.GetAllArticles()");
            try
            {
                return _articleAdapter.GetById(id);
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

        public void UpdateArticle(Article article)
        {
            _logger.LogInformation($"{typeof(ArticleService).FullName}.UpdateArticle({article.Id})");
            try
            {
                _articleAdapter.Update(article);
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
