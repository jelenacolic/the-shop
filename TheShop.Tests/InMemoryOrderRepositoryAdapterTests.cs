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
    public class InMemoryOrderRepositoryAdapterTests
    {
        #region Public methods
        [Fact]
        public void Delete_ValidCall()
        {
            long orderId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()))
                    .Verifiable();

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                adapter.Delete(orderId);

                mock.Mock<IOrderInMemoryRepository>().Verify(x => x.Delete(orderId), Times.Exactly(1));

            };
        }

        [Fact]
        public void Delete_InvalidCall()
        {
            long OrderId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.Delete(OrderId));
            };
        }

        [Fact]
        public void Insert_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Order>()))
                .Returns(GetOrderEntity(1,1,1));

                var OrderToInsertAdapter = GetOrderBusiness(1, 1, 1);

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                var expected = GetOrderBusiness(1,1,1);
                var actual = adapter.Insert(OrderToInsertAdapter);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Buyer.Id, actual.Buyer.Id);
                Assert.Equal(expected.Article.Id, actual.Buyer.Id);
            };
        }

        [Fact]
        public void Insert_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            { 
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Order>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                var OrderToInsertAdapter = GetOrderBusiness(1, 1, 1);

                Assert.Throws<LoggedException>(() => adapter.Insert(OrderToInsertAdapter));
            };
        }

        [Fact]
        public void Update_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var OrderToUpdateEntity = GetOrderEntity(1,1,1);

                mock.Mock<IOrderInMemoryRepository>()
                .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Order>())).Verifiable();

                var OrderToUpdateAdapter = GetOrderBusiness(1, 1,1);

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();
                adapter.Update(OrderToUpdateAdapter);

                mock.Mock<IOrderInMemoryRepository>().Verify(x => x.Update(It.IsAny<EntityModels.InMemory.Order>()), Times.Exactly(1));
            };
        }

        [Fact]
        public void Update_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Order>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                var OrderToUpdateAdapter = GetOrderBusiness(1, 1,1);

                Assert.Throws<LoggedException>(() => adapter.Update(OrderToUpdateAdapter));
            };
        }

        [Fact]
        public void GetAll_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                .Setup(x => x.GetAll())
                .Returns(GetAllOrderEntities());

                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.GetById(It.IsAny<long>()))
                    .Returns(GetBuyerEntity());

                mock.Mock<IInventoryInMemoryRepository>()
                    .Setup(x => x.GetById(It.IsAny<long>()))
                    .Returns(GetInventoryEntity());

                mock.Mock<IArticleInMemoryRepository>()
                    .Setup(x => x.GetById(It.IsAny<long>()))
                    .Returns(GetArticleEntity());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                var actual = adapter.GetAll();
                var expected = GetAllOrderBusiness();

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
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.GetAll())
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetAll());
            };
        }

        [Fact]
        public void GetById_ValidCall()
        {
            var articleId = 2;
            var buyerId = 1;
            var inventoryId = 1;
            var orderId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                .Setup(x => x.GetById(orderId))
                .Returns(GetOrderEntity(orderId,inventoryId,buyerId));

                mock.Mock<IBuyerInMemoryRepository>()
                .Setup(x => x.GetById(buyerId))
                .Returns(GetBuyerEntity());

                mock.Mock<IArticleInMemoryRepository>()
                .Setup(x => x.GetById(articleId))
                .Returns(GetArticleEntity());

                mock.Mock<IInventoryInMemoryRepository>()
                .Setup(x => x.GetById(inventoryId))
                .Returns(GetInventoryEntity());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                var actual = adapter.GetById(orderId);
                var expected = GetOrderBusiness(orderId,inventoryId,buyerId);

                Assert.Equal(actual.Id, expected.Id);
            };
        }

        [Fact]
        public void GetById_InvalidCall()
        {
            var id = 1;
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IOrderInMemoryRepository>()
                    .Setup(x => x.GetById(id))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryOrderRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryOrderRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetById(id));
            };
        }
        #endregion

        #region Private methods
        private EntityModels.InMemory.Order GetOrderEntity(long id, long inventoryId, long buyerId)
        {
            return new EntityModels.InMemory.Order()
            {
                Id = id,
                BuyerId = buyerId,
                InventoryId = inventoryId,
                DateSold = DateTime.UtcNow
            };
        }

        private BusinessModels.Order GetOrderBusiness(long id, long inventoryId, long buyerId)
        {
            return new BusinessModels.Order()
            {
                Id = id,
                Article = new BusinessModels.Article()
                {
                    Id = 1,
                    InventoryId = inventoryId,
                    EAN = "0123456789123",
                    Name = "Article1",
                    Price = 100,
                    Quantity = 2
                },
                Buyer = new BusinessModels.Buyer()
                {
                    Id = buyerId,
                    Name = "Buyer1"
                },
                Date = DateTime.UtcNow
            };
        }

        private IList<EntityModels.InMemory.Order> GetAllOrderEntities()
        {
            return new List<EntityModels.InMemory.Order>()
            {
                new EntityModels.InMemory.Order()
                {
                    Id = 1,
                    BuyerId = 1,
                    InventoryId = 2,
                    DateSold = DateTime.UtcNow
                },
                new EntityModels.InMemory.Order()
                {
                    Id = 2,
                    BuyerId = 2,
                    InventoryId = 3,
                    DateSold = DateTime.UtcNow
                },
                new EntityModels.InMemory.Order()
                {
                    Id = 3,
                    BuyerId = 1,
                    InventoryId = 3,
                    DateSold = DateTime.UtcNow
                }
            };
        }

        private IList<BusinessModels.Order> GetAllOrderBusiness()
        {
            return new List<BusinessModels.Order>()
            {
                new BusinessModels.Order()
                {
                    Id = 1,
                    Article = new BusinessModels.Article()
                    {
                        Id = 1,
                        InventoryId = 2,
                        EAN = "0123456789123",
                        Name = "Article1",
                        Price = 100,
                        Quantity = 2
                    },
                    Buyer = new BusinessModels.Buyer()
                    {
                        Id = 1,
                        Name = "Buyer1"
                    },
                    Date = DateTime.UtcNow
                },
                new BusinessModels.Order()
                {
                    Id = 2,
                    Article = new BusinessModels.Article()
                    {
                        Id = 2,
                        InventoryId = 3,
                        EAN = "1234567890123",
                        Name = "Article2",
                        Price = 50,
                        Quantity = 1
                    },
                    Buyer = new BusinessModels.Buyer()
                    {
                        Id = 2,
                        Name = "Buyer2"
                    },
                    Date = DateTime.UtcNow
                },
                new BusinessModels.Order()
                {
                    Id = 3,
                    Article = new BusinessModels.Article()
                    {
                        Id = 2,
                        InventoryId = 3,
                        EAN = "01234567890123",
                        Name = "Article2",
                        Price = 50,
                        Quantity = 1
                    },
                    Buyer = new BusinessModels.Buyer()
                    {
                        Id = 1,
                        Name = "Buyer1"
                    },
                    Date = DateTime.UtcNow
                }
            };
        }

        private EntityModels.InMemory.Article GetArticleEntity()
        {
            return new EntityModels.InMemory.Article()
            {
                Id = 2,
                EAN = "0123456789123",
                Name = "Article2"
            };
        }
        private EntityModels.InMemory.Buyer GetBuyerEntity()
        {
            return new EntityModels.InMemory.Buyer()
            {
                Id = 1,
                Name = "Buyer1"
            };
        }

        private EntityModels.InMemory.Inventory GetInventoryEntity()
        {
            return new EntityModels.InMemory.Inventory()
            {
                Id = 1,
                ArticleId = 2,
                SupplierId = 1  
            };
        }
        #endregion
    }
}
