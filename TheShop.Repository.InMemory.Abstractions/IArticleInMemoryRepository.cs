using System;
using System.Collections.Generic;
using TheShop.EntityModels.InMemory;

namespace TheShop.Repository.InMemory.Abstractions
{
    public interface IArticleInMemoryRepository
    {
        Article GetById(long id);

        Article GetByEan(string ean);

        Article Insert(Article entity);

        void Update(Article entity);

        void Delete(long id);

        IList<Article> GetAll();

        IList<Article> GetAll(Func<Article, bool> predicate);
    }
}
