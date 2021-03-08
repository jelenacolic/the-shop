using System.Collections.Generic;
using TheShop.BusinessModels;

namespace TheShop.Services.Abstractions
{
    public interface IArticleService
    {
        Article AddArticle(Article article);

        void UpdateArticle(Article article);

        Article GetArticle(string ean);

        Article GetArticle(long id);

        IList<Article> GetAllArticles();
    }
}
