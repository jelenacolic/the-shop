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
    public class InMemoryBuyerRepositoryAdapterTests
    {
        #region Public methods
        [Fact]
        public void Delete_ValidCallTrue()
        {
            long buyerId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IOrderInMemoryRepository>()
                   .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Order, bool>>()))
                   .Returns(new List<EntityModels.InMemory.Order>());


                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var actual = adapter.Delete(buyerId);

                Assert.True(actual);
            };
        }

        [Fact]
        public void Delete_ValidCallFalse()
        {
            long buyerId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()));

                mock.Mock<IOrderInMemoryRepository>()
                   .Setup(x => x.GetAll(It.IsAny<Func<EntityModels.InMemory.Order, bool>>()))
                   .Returns(GetOrderEntities(buyerId));

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var actual = adapter.Delete(buyerId);

                Assert.False(actual);

            };
        }

        [Fact]
        public void Delete_InvalidCall()
        {
            long buyerId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.Delete(It.IsAny<long>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.Delete(buyerId));
            };
        }

        [Fact]
        public void Insert_ValidCall()
        {
            var name = "Buyer1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Buyer>()))
                .Returns(GetBuyerEntity(2, name));

                var BuyerToInsertAdapter = GetBuyerBusiness(1, name);

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var expected = GetBuyerBusiness(2, name);
                var actual = adapter.Insert(BuyerToInsertAdapter);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);

            };
        }

        [Fact]
        public void Insert_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            { 
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.Insert(It.IsAny<EntityModels.InMemory.Buyer>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var BuyerToInsertAdapter = GetBuyerBusiness(1, "Buyer1");

                Assert.Throws<LoggedException>(() => adapter.Insert(BuyerToInsertAdapter));
            };
        }

        [Fact]
        public void Update_ValidCall()
        {
            var name = "Buyer1";

            using (var mock = AutoMock.GetLoose())
            {
                var BuyerToUpdateEntity = GetBuyerEntity(1, name);

                mock.Mock<IBuyerInMemoryRepository>()
                .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Buyer>())).Verifiable();

                var BuyerToUpdateAdapter = GetBuyerBusiness(1, name);

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();
                adapter.Update(BuyerToUpdateAdapter);

                mock.Mock<IBuyerInMemoryRepository>().Verify(x => x.Update(It.IsAny<EntityModels.InMemory.Buyer>()), Times.Exactly(1));
            };
        }

        [Fact]
        public void Update_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.Update(It.IsAny<EntityModels.InMemory.Buyer>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var BuyerToUpdateAdapter = GetBuyerBusiness(1, "Buyer1");

                Assert.Throws<LoggedException>(() => adapter.Update(BuyerToUpdateAdapter));
            };
        }

        [Fact]
        public void GetAll_ValidCall()
        {


            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                .Setup(x => x.GetAll())
                .Returns(GetAllBuyerEntities());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var actual = adapter.GetAll();
                var expected = GetAllBuyerBusiness();

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(actual[i].Id, expected[i].Id);
                    Assert.Equal(actual[i].Name, expected[i].Name);
                }
            };
        }

        [Fact]
        public void GetAll_InvalidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.GetAll())
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetAll());
            };
        }

        [Fact]
        public void GetById_ValidCall()
        {
            var id = 1;
            var name = "Buyer 1";

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                .Setup(x => x.GetById(1))
                .Returns(GetBuyerEntity(id,name));

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                var actual = adapter.GetById(1);
                var expected = GetBuyerBusiness(id,name);

                Assert.Equal(actual.Id, expected.Id);
                Assert.Equal(actual.Name, expected.Name);
            };
        }

        [Fact]
        public void GetById_InvalidCall()
        {
            var id = 1;
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IBuyerInMemoryRepository>()
                    .Setup(x => x.GetById(id))
                    .Throws(new Exception());

                mock.Mock<ILogger<InMemoryBuyerRepositoryAdapter>>();

                var adapter = mock.Create<InMemoryBuyerRepositoryAdapter>();

                Assert.Throws<LoggedException>(() => adapter.GetById(id));
            };
        }
        #endregion

        #region Private methods
        private EntityModels.InMemory.Buyer GetBuyerEntity(long id, string name)
        {
            return new EntityModels.InMemory.Buyer()
            {
                Id = id,
                Name = name
            };
        }

        private BusinessModels.Buyer GetBuyerBusiness( long id, string name)
        {
            return new BusinessModels.Buyer()
            {
                Id = id,
                Name = name
            };
        }

        private IList<EntityModels.InMemory.Buyer> GetAllBuyerEntities()
        {
            return new List<EntityModels.InMemory.Buyer>()
            {
                new EntityModels.InMemory.Buyer()
                {
                    Id = 1,
                    Name = "Buyer1"
                },
                new EntityModels.InMemory.Buyer()
                {
                    Id = 2,
                    Name = "Buyer2"
                },
                new EntityModels.InMemory.Buyer()
                {
                    Id = 3,
                    Name = "Buyer3"
                }
            };
        }

        private IList<BusinessModels.Buyer> GetAllBuyerBusiness()
        {
            return new List<BusinessModels.Buyer>()
            {
                new BusinessModels.Buyer()
                {
                    Id = 1,
                    Name = "Buyer1"
                },
                new BusinessModels.Buyer()
                {
                    Id = 2,
                    Name = "Buyer2"
                },
                new BusinessModels.Buyer()
                {
                    Id = 3,
                    Name = "Buyer3"
                }
            };
        }

        private IList<EntityModels.InMemory.Order> GetOrderEntities(long buyerId)
        {
            return new List<EntityModels.InMemory.Order>()
            {
                new EntityModels.InMemory.Order()
                {
                    Id = 1,
                    BuyerId = buyerId,
                    InventoryId = 1
                },
                new EntityModels.InMemory.Order()
                {

                    Id = 2,
                    BuyerId = buyerId,
                    InventoryId = 2
                },
                new EntityModels.InMemory.Order()
                {

                    Id = 3,
                    BuyerId = buyerId,
                    InventoryId = 3
                }
            };
        }
        #endregion
    }
}
