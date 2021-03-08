using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.BusinessModels;
using TheShop.Exceptions;
using TheShop.Services;
using Xunit;

namespace TheShop.Tests
{
    public class SupplierServiceTests
    {
        #region Public methods
        [Fact]
        public void OrderArticle_ValidCallFull()
        {
            string ean = "1234567890123";
            double maxExpectedPrice = 100;
            int inventoryId = 5;

            using (var mock = AutoMock.GetLoose())
            {
                var mockedSupplierRepositoryAdapter = mock.Mock<ISupplierRepositoryAdapter>();

                mockedSupplierRepositoryAdapter.Setup(x => x.GetSuppliersWithArticleInInventory(It.IsAny<string>(), It.IsAny<double>()))
                    .Returns(GetSampleSuppliersWithArticleInInventory(ean, maxExpectedPrice, inventoryId));

                mockedSupplierRepositoryAdapter.Setup(x => x.DecreaseArticleQuantity(inventoryId)).Verifiable();

                mock.Mock<ILogger<SupplierService>>();

                var service = mock.Create<SupplierService>();
                var expected = GetArticle(ean, maxExpectedPrice, inventoryId);
                var actual = service.OrderArticle(ean, maxExpectedPrice);

                mockedSupplierRepositoryAdapter.Verify(x => x.DecreaseArticleQuantity(inventoryId), Times.Exactly(1));

                
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.EAN, actual.EAN);
                Assert.Equal(expected.InventoryId, actual.InventoryId);
                Assert.Equal(expected.Price, actual.Price);
                Assert.Equal(expected.Quantity, actual.Quantity);
            };
        }

        [Fact]
        public void OrderArticle_ValidCallNoSuppliers()
        {
            string ean = "1234567890123";
            double maxExpectedPrice = 100;

            using (var mock = AutoMock.GetLoose())
            {
                var mockedSupplierRepositoryAdapter = mock.Mock<ISupplierRepositoryAdapter>();

                mockedSupplierRepositoryAdapter.Setup(x => x.GetSuppliersWithArticleInInventory(It.IsAny<string>(), It.IsAny<double>()))
                    .Returns(new List<Supplier>());

                mock.Mock<ILogger<SupplierService>>();

                var service = mock.Create<SupplierService>();
                var actual = service.OrderArticle(ean, maxExpectedPrice);

                Assert.Null(actual);
            };
        }

        [Fact]
        public void OrderArticle_InvalidCallLoggedException()
        {
            string ean = "1234567890123";
            double maxExpectedPrice = 100;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ISupplierRepositoryAdapter>()
                    .Setup(x => x.GetSuppliersWithArticleInInventory(It.IsAny<string>(), It.IsAny<double>()))
                    .Throws(new Exception());

                mock.Mock<ILogger<SupplierService>>();

                var service = mock.Create<SupplierService>();
                
                Assert.Throws<LoggedException>(() => service.OrderArticle(ean, maxExpectedPrice));

            };
        }

        #endregion

        #region Private methods
        private IList<Supplier> GetSampleSuppliersWithArticleInInventory(string ean, double maxExpectedPrice, int inventoryId)
        {
            return new List<Supplier>()
            {
                new Supplier()
                {
                    Id = 1,
                    Name = "Supplier1",
                    Inventory = new Dictionary<long, Article>()
                    {
                        {1, new Article()
                            {
                                Id = 1,
                                EAN = ean,
                                Name = "Article1",
                                InventoryId = 1,
                                Price = maxExpectedPrice - 10,
                                Quantity = 2
                            }
                        }
                    }
                },
                new Supplier()
                {
                    Id = 2,
                    Name = "Supplier2",
                    Inventory = new Dictionary<long, Article>() 
                    {
                        {1, new Article()
                            {
                                Id = 1,
                                EAN = ean,
                                Name = "Article1",
                                InventoryId = inventoryId,
                                Price = maxExpectedPrice - 20,
                                Quantity = 1
                            }
                        }
                    }
                }
            };
        }

        private Article GetArticle(string ean, double maxExpectedPrice, int inventoryId)
        {
            return new Article()
            {
                Id = 1,
                EAN = ean,
                Name = "Article1",
                InventoryId = inventoryId,
                Price = maxExpectedPrice - 20,
                Quantity = 1
            };
        }

        #endregion
    }
}
