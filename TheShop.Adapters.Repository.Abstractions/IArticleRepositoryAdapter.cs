using System.Collections.Generic;
using TheShop.BusinessModels;

namespace TheShop.Adapters.Repository.Abstractions
{
    public interface IArticleRepositoryAdapter
    {
        Article GetById(long id);

        Article GetByEan(string ean);

        Article Insert(Article article);

        void Update(Article article);

        bool Delete(long id);

        IList<Article> GetAll();

    }
}
