using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using TheShop.Adapters.Repository.InMemory;
using TheShop.Exceptions;
using TheShop.Repository.InMemory.Abstractions;
using Xunit;

namespace TheShop.Tests
{
    public class InMemoryArticleRepositoryAdapterTests
    {
        #region Public methods
        [Fact]
        public void Delete_ValidCallTrue()
        {
            long articleId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IInventoryInMemoryRepository>()
                   .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                   .Returns(new List<EntityModels.InMemory.Inventory>());

                
                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var actual = adapter.Delete(articleId);

                Assert.True(actual);

            };
        }

        [Fact]
        public void Delete_ValidCallFalse()
        {
            long articleId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                    .Returns(GetInventoryEntities(articleId));


                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var actual = adapter.Delete(articleId);


                Assert.False(actual);

            };
        }

        [Fact]
        public void Delete_InvalidCall()
        {
            long articleId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.Delete(articleId));
            };
        }

        [Fact]
        public void Insert_ValidCall()
        {
            var ean = "1234567890123";
            var name = "Article1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Article>()))
                .Returns(GetArticleEntity(ean, 2, name));

                var articleToInsertAdapter = GetArticleBusiness(ean, 1, name);

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var expected = GetArticleBusiness(ean, 2, name);
                var actual = adapter.Insert(articleToInsertAdapter);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.EAN, actual.EAN);
                Assert.Equal(expected.Name, actual.Name);

            };
        }

        [Fact]
        public void Insert_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            { 
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Article>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var articleToInsertAdapter = GetArticleBusiness("1234567890123", 1, "Article1");

                Assert.Throws<LoggedException>(() => adapter.Insert(articleToInsertAdapter));
            };
        }

        [Fact]
        public void Update_ValidCall()
        {
            var ean = "1234567890123";
            var name = "Article1";

            using (var mock = AutoMock.GetLoose())
            {
                var articleToUpdateEntity = GetArticleEntity(ean, 1, name);

                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Article>())).Verifiable();

                var articleToUpdateAdapter = GetArticleBusiness(ean, 1, name);

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();
                adapter.Update(articleToUpdateAdapter);

                mock.Mock<IArticleInMemoryRepository>().Verify(x => x.Update(It.IsAny<EntityModels.InMemory.Article>()), Times.Exactly(1));
            };
        }

        [Fact]
        public void Update_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Article>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var articleToUpdateAdapter = GetArticleBusiness("1234567890123", 1, "Article1");

                Assert.Throws<LoggedException>(() => adapter.Update(articleToUpdateAdapter));
            };
        }

        [Fact]
        public void GetAll_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.GetAll())
                .Returns(GetAllArticleEntities());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var actual = adapter.GetAll();
                var expected = GetAllArticleBusiness();

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(actual[i].Id, expected[i].Id);
                    Assert.Equal(actual[i].EAN, expected[i].EAN);
                    Assert.Equal(actual[i].Name, expected[i].Name);
                }
            };
        }

        [Fact]
        public void GetAll_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetAll())
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetAll());
            };
        }

        [Fact]
        public void GetById_ValidCall()
        {
            var id = 1;
            var ean = "0123456789123";
            var name = "Article 1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.GetById(1))
                .Returns(GetArticleEntity(ean,id,name));

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var actual = adapter.GetById(1);
                var expected = GetArticleBusiness(ean,id,name);

                Assert.Equal(actual.Id, expected.Id);
                Assert.Equal(actual.EAN, expected.EAN);
                Assert.Equal(actual.Name, expected.Name);
            };
        }

        [Fact]
        public void GetById_InvalidCall()
        {
            var id = 1;
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetById(id))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetById(id));
            };
        }

        [Fact]
        public void GetByEan_ValidCall()
        {
            var id = 1;
            var ean = "0123456789123";
            var name = "Article 1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.GetByEan(ean))
                .Returns(GetArticleEntity(ean, id, name));

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                var actual = adapter.GetByEan(ean);
                var expected = GetArticleBusiness(ean, id, name);

                Assert.Equal(actual.Id, expected.Id);
                Assert.Equal(actual.EAN, expected.EAN);
                Assert.Equal(actual.Name, expected.Name);
            };
        }

        [Fact]
        public void GetByEan_InvalidCall()
        {
            var ean = "0123456789123";
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetByEan(ean))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryArticleRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryArticleRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetByEan(ean));
            };
        }
        #endregion

        #region Private methods
        private EntityModels.InMemory.Article GetArticleEntity(string ean, long id, string name)
        {
            return new EntityModels.InMemory.Article()
            {
                Id = id,
                EAN = ean,
                Name = name
            };
        }

        private BusinessModels.Article GetArticleBusiness(string ean, long id, string name)
        {
            return new BusinessModels.Article()
            {
                Id = id,
                EAN = ean,
                Name = name
            };
        }

        private IList<EntityModels.InMemory.Article> GetAllArticleEntities()
        {
            return new List<EntityModels.InMemory.Article>()
            {
                new EntityModels.InMemory.Article()
                {
                    Id = 1,
                    EAN = "1234567890123",
                    Name = "Article1"
                },
                new EntityModels.InMemory.Article()
                {
                    Id = 2,
                    EAN = "0987654321098",
                    Name = "Article2"
                },
                new EntityModels.InMemory.Article()
                {
                    Id = 3,
                    EAN = "1238904567777",
                    Name = "Article3"
                }
            };
        }

        private IList<BusinessModels.Article> GetAllArticleBusiness()
        {
            return new List<BusinessModels.Article>()
            {
                new BusinessModels.Article()
                {
                    Id = 1,
                    EAN = "1234567890123",
                    Name = "Article1"
                },
                new BusinessModels.Article()
                {
                    Id = 2,
                    EAN = "0987654321098",
                    Name = "Article2"
                },
                new BusinessModels.Article()
                {
                    Id = 3,
                    EAN = "1238904567777",
                    Name = "Article3"
                }
            };
        }

        private IList<EntityModels.InMemory.Inventory> GetInventoryEntities(long articleId)
        {
            return new List<EntityModels.InMemory.Inventory>()
            {
                new EntityModels.InMemory.Inventory()
                {
                    Id = 1,
                    SupplierId = 1,
                    ArticleId = articleId,
                    Price = 100,
                    Quantity = 2
                },
                new EntityModels.InMemory.Inventory()
                {

                    Id = 2,
                    SupplierId = 2,
                    ArticleId = articleId,
                    Price = 110,
                    Quantity = 1
                },
                new EntityModels.InMemory.Inventory()
                {

                    Id = 3,
                    SupplierId = 3,
                    ArticleId = articleId,
                    Price = 90,
                    Quantity = 3
                }
            };
        }
        #endregion
    }
}
