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
    public class InMemorySupplierRepositoryAdapterTests
    {
        #region Public methods
        [Fact]
        public void Delete_ValidCallTrue()
        {
            long supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                    .Returns(GetInventoryEntities(supplierId));

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var actual = adapter.Delete(supplierId);

                Assert.True(actual);

            };
        }

        [Fact]
        public void Delete_ValidCallFalse()
        {
            long supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                    .Returns(new List<EntityModels.InMemory.Inventory>());


                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var actual = adapter.Delete(supplierId);

                Assert.False(actual);
            };
        }

        [Fact]
        public void Delete_InvalidCall()
        {
            long SupplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.Delete(SupplierId));
            };
        }

        [Fact]
        public void Insert_ValidCall()
        {
            var id = 1;
            var name = "Supplier1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Supplier>()))
                .Returns(GetSupplierEntity(id, name));

                var SupplierToInsertAdapter = GetSupplierBusiness(id, name);

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var expected = GetSupplierBusiness(id, name);
                var actual = adapter.Insert(SupplierToInsertAdapter);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            };
        }

        [Fact]
        public void Insert_InvalidCall()
        {
            var id = 1;
            var name = "Supplier1";

            using (var mock = AutoMock.GetLoose())
            { 
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Supplier>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var SupplierToInsertAdapter = GetSupplierBusiness(id, name);

                Assert.Throws<LoggedException>(() => adapter.Insert(SupplierToInsertAdapter));
            };
        }

        [Fact]
        public void Update_ValidCall()
        {
            var name = "Supplier1";
            var id = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var SupplierToUpdateEntity = GetSupplierEntity(id, name);

                mock.Mock<ISupplierInMemoryRepository>()
                .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Supplier>())).Verifiable();

                var SupplierToUpdateAdapter = GetSupplierBusiness(id, name);

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();
                adapter.Update(SupplierToUpdateAdapter);

                mock.Mock<ISupplierInMemoryRepository>().Verify(x => x.Update(It.IsAny<EntityModels.InMemory.Supplier>()), Times.Exactly(1));
            };
        }

        [Fact]
        public void Update_InvalidCall()
        {
            var id = 1;
            var name = "Supplier1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Supplier>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var SupplierToUpdateAdapter = GetSupplierBusiness(id, name);

                Assert.Throws<LoggedException>(() => adapter.Update(SupplierToUpdateAdapter));
            };
        }

        [Fact]
        public void GetAll_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                .Setup(x => x.GetAll())
                .Returns(GetAllSupplierEntities());

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                    .Returns(GetInventoryEntities(0));

                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Article, bool>>()))
                    .Returns(GetArticleEntities());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var actual = adapter.GetAll();
                var expected = GetAllSupplierBusiness();

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(actual[i].Id, expected[i].Id);
                }
            };
        }

        [Fact]
        public void GetAll_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.GetAll())
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetAll());
            };
        }

        [Fact]
        public void GetById_ValidCall()
        { 
            var supplierId = 1;

            var name = "Supplier1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                .Setup(x => x.GetById(supplierId))
                .Returns(GetSupplierEntity(supplierId, name));

                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Article, bool>>()))
                .Returns(GetArticleEntities());

                mock.Mock<IInventoryInMemoryRepository>()
                .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                .Returns(GetInventoryEntities(supplierId));

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var actual = adapter.GetById(supplierId);
                var expected = GetSupplierBusiness(supplierId, name);

                Assert.Equal(actual.Id, expected.Id);
            };
        }

        [Fact]
        public void GetById_InvalidCall()
        {
            var id = 1;
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.GetById(id))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetById(id));
            };
        }

        [Fact]
        public void DecreaseArticleQuantity_ValidCall()
        {
            var inventoryId = 1;
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var inventoryEntity = GetInventoryEntity(inventoryId, articleId, supplierId);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetById(inventoryId))
                    .Returns(inventoryEntity);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.Update(inventoryEntity))
                    .Verifiable();

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                adapter.DecreaseArticleQuantity(inventoryId);

                mock.Mock<IInventoryInMemoryRepository>().Verify(x => x.Update(inventoryEntity), Times.Exactly(1));
            };
        }

        [Fact]
        public void DecreaseArticleQuantity_InvalidCall()
        {
            var inventoryId = 1;
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var inventoryEntity = GetInventoryEntity(inventoryId, articleId, supplierId);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetById(inventoryId))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.DecreaseArticleQuantity(inventoryId));
            };
        }

        [Fact]
        public void IncreaseArticleQuantity_ValidCall()
        {
            var inventoryId = 1;
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var inventoryEntity = GetInventoryEntity(inventoryId, articleId, supplierId);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetById(inventoryId))
                    .Returns(inventoryEntity);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.Update(inventoryEntity))
                    .Verifiable();

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                adapter.IncreaseArticleQuantity(inventoryId);

                mock.Mock<IInventoryInMemoryRepository>().Verify(x => x.Update(inventoryEntity), Times.Exactly(1));
            };
        }

        [Fact]
        public void IncreaseArticleQuantity_InvalidCall()
        {
            var inventoryId = 1;
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var inventoryEntity = GetInventoryEntity(inventoryId, articleId, supplierId);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetById(inventoryId))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.IncreaseArticleQuantity(inventoryId));
            };
        }

        [Fact]
        public void AddArticleToSupplierInventory_ValidCall()
        {
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Inventory>()))
                    .Verifiable();

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                adapter.AddArticleToSupplierInventory(supplierId, GetArticleBusiness(articleId));

                mock.Mock<IInventoryInMemoryRepository>().Verify(x => x.Insert(It.IsAny<EntityModels.InMemory.Inventory>()), Times.Exactly(1));
            };
        }

        [Fact]
        public void AddArticleToSupplierInventory_InvalidCall()
        {
            var inventoryId = 1;
            var articleId = 1;
            var supplierId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                var inventoryEntity = GetInventoryEntity(inventoryId, articleId, supplierId);

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Inventory>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.AddArticleToSupplierInventory(supplierId, GetArticleBusiness(articleId)));
            };
        }

        [Fact]
        public void GetSuppliersWithArticleInInventory_ValidCall()
        {
            var ean = "0123456789213";
            var maxExpectedPrice = 100;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetByEan(ean))
                    .Returns(GetArticleEntity(ean));

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Inventory, bool>>()))
                    .Returns(GetInventoryEntitiesForSuppliersWithArticle(ean));


                mock.Mock<ISupplierInMemoryRepository>()
                    .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Supplier, bool>>()))
                    .Returns(GetAllSupplierEntities());


                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                var actual = adapter.GetSuppliersWithArticleInInventory(ean, maxExpectedPrice);

                var expected = new List<BusinessModels.Supplier>()
                {
                    new BusinessModels.Supplier()
                    {
                        Id = 1,
                        Name = "Supplier1",
                        Inventory = new Dictionary<long, BusinessModels.Article>()
                        {
                            {
                                1,
                                new BusinessModels.Article()
                                {
                                    EAN = ean,
                                    Id = 1,
                                    InventoryId = 1,
                                    Name = "Article1",
                                    Price = 100,
                                    Quantity = 2
                                }
                            }
                        }
                    }
                };

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].Id, actual[i].Id);

                    for (int j = 9; j < expected[i].Inventory.Count; j++)
                    {
                        Assert.Equal(expected[i].Inventory[j].Id, actual[i].Inventory[j].Id);
                        Assert.Equal(expected[i].Inventory[j].Name, actual[i].Inventory[j].Name);
                        Assert.Equal(expected[i].Inventory[j].EAN, actual[i].Inventory[j].EAN);
                        Assert.Equal(expected[i].Inventory[j].Price, actual[i].Inventory[j].Price);
                        Assert.Equal(expected[i].Inventory[j].Quantity, actual[i].Inventory[j].Quantity);
                    }
                }
            };
        }

        [Fact]
        public void GetSuppliersWithArticleInInventory_InvalidCall()
        {
            var ean = "0123456789213";
            var maxExpectedPrice = 100;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetByEan(ean))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemorySupplierRepositoryAdapter>>();

                var adapter = mock.Create<InMemorySupplierRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetSuppliersWithArticleInInventory(ean, maxExpectedPrice));
            };
        }
        #endregion

        #region Private methods
        private EntityModels.InMemory.Supplier GetSupplierEntity(long id, string name)
        {
            return new EntityModels.InMemory.Supplier()
            {
                Id = id,
                Name = name
            };
        }

        private BusinessModels.Supplier GetSupplierBusiness(long id, string name)
        {
            return new BusinessModels.Supplier()
            {
                Id = id,
                Name = name
            };
        }

        private IList<EntityModels.InMemory.Supplier> GetAllSupplierEntities()
        {
            return new List<EntityModels.InMemory.Supplier>()
            {
                new EntityModels.InMemory.Supplier()
                {
                    Id = 1,
                    Name = "Supplier1"
                },
                new EntityModels.InMemory.Supplier()
                {
                    Id = 2,
                    Name = "Supplier2"
                },
                new EntityModels.InMemory.Supplier()
                {
                    Id = 3,
                    Name = "Supplier3"
                }
            };
        }

        private IList<BusinessModels.Supplier> GetAllSupplierBusiness()
        {
            return new List<BusinessModels.Supplier>()
            {
                new BusinessModels.Supplier()
                {
                    Id = 1,
                    Name = "Supplier1"
                },
                new BusinessModels.Supplier()
                {
                    Id = 2,
                    Name = "Supplier2"
                },
                new BusinessModels.Supplier()
                {
                    Id = 3,
                    Name = "Supplier3"
                }
            };
        }

        private EntityModels.InMemory.Article GetArticleEntity(string ean = null)
        {
            return new EntityModels.InMemory.Article()
            {
                Id = 2,
                EAN = ean != null ? ean : "0123456789123",
                Name = "Article2"
            };
        }
        private EntityModels.InMemory.Inventory GetInventoryEntity(long inventoryId, long articleId, long supplierId, long quantity = default, double price = default)
        {
            return new EntityModels.InMemory.Inventory()
            {
                Id = inventoryId,
                ArticleId = articleId,
                SupplierId = supplierId,
                Price = price,
                Quantity = quantity
            };
        }

        private IList<EntityModels.InMemory.Article> GetArticleEntities()
        {
            return new List<EntityModels.InMemory.Article>()
            {
                new EntityModels.InMemory.Article()
                {
                    Id = 1,
                    Name = "Article1"
                },
                new EntityModels.InMemory.Article()
                {
                    Id = 2,
                    Name = "Article2"
                },
                new EntityModels.InMemory.Article()
                {
                    Id = 3,
                    Name = "Article3"
                }
            };
        }

        private IList<EntityModels.InMemory.Inventory> GetInventoryEntities(long supplierId = default)
        {
            return new List<EntityModels.InMemory.Inventory>()
            {
                new EntityModels.InMemory.Inventory()
                {
                    Id = 1,
                    SupplierId = supplierId,
                    ArticleId = 1,
                    Price = 100,
                    Quantity = 2
                },
                new EntityModels.InMemory.Inventory()
                {
                    Id = 2,
                    SupplierId = supplierId,
                    ArticleId = 2,
                    Price = 10,
                    Quantity = 5
                },
                new EntityModels.InMemory.Inventory()
                {
                    Id = 3,
                    SupplierId = supplierId,
                    ArticleId = 3,
                    Price = 200,
                    Quantity = 1
                }
            };
        }

        private BusinessModels.Article GetArticleBusiness(long articleId)
        {
            return new BusinessModels.Article()
            {
                EAN = "012345678123",
                Id = articleId,
                InventoryId = 1,
                Name = "Article1",
                Price = 50,
                Quantity = 2
            };
        }

        private IList<EntityModels.InMemory.Inventory> GetInventoryEntitiesForSuppliersWithArticle(string ean)
        {
            return new List<EntityModels.InMemory.Inventory>()
            {
                new EntityModels.InMemory.Inventory()
                {
                    Id = 1,
                    SupplierId = 1,
                    ArticleId = 1,
                    Price = 100,
                    Quantity = 2
                },
                new EntityModels.InMemory.Inventory()
                {
                    Id = 2,
                    SupplierId = 2,
                    ArticleId = 1,
                    Price = 150,
                    Quantity = 5
                },
                new EntityModels.InMemory.Inventory()
                {
                    Id = 3,
                    SupplierId = 3,
                    ArticleId = 1,
                    Price = 200,
                    Quantity = 1
                }
            };
        }
        #endregion
    }
}
