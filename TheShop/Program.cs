using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using TheShop.Adapters.Repository.Abstractions;
using TheShop.Adapters.Repository.InMemory;
using TheShop.BusinessModels;
using TheShop.Repository.InMemory;
using TheShop.Repository.InMemory.Abstractions;
using TheShop.Services;
using TheShop.Services.Abstractions;

namespace TheShop
{
	class Program
	{
		static void Main(string[] args)
		{
			using IHost host = CreateHostBuilder(args).Build();

			IShopService shopService = host.Services.GetService<IShopService>();
			IArticleService articleService = host.Services.GetService<IArticleService>();
			ISupplierService supplierService = host.Services.GetService<ISupplierService>();
			IBuyerService buyerService = host.Services.GetService<IBuyerService>();

			// Add suppliers for example
			AddSuppliers(supplierService);

			// Add articles for example
			AddArticles(articleService);

			// Add inventory for example
			AddInventory(supplierService, articleService);

			// Add buyers for example
			AddBuyers(buyerService);

			// Order and sell article as an example
			OrderAndSellArticle(supplierService, buyerService, shopService, articleService);

			// Get and print existing article as an example
			GetAndPrintArticle(articleService, 1);

			// Get and print non existing article as an example
			GetAndPrintArticle(articleService, 12);

			Console.ReadKey();
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) =>
					services.AddTransient<IShopService, ShopService>()
							.AddTransient<ISupplierService, SupplierService>()
							.AddTransient<IArticleService, ArticleService>()
							.AddTransient<IBuyerService, BuyerService>()
							.AddTransient<IArticleRepositoryAdapter, InMemoryArticleRepositoryAdapter>()
							.AddTransient<IBuyerRepositoryAdapter, InMemoryBuyerRepositoryAdapter>()
							.AddTransient<ISupplierRepositoryAdapter, InMemorySupplierRepositoryAdapter>()
							.AddTransient<IOrderRepositoryAdapter, InMemoryOrderRepositoryAdapter>()
							.AddSingleton<IArticleInMemoryRepository, ArticleInMemoryRepository>()
							.AddSingleton<IBuyerInMemoryRepository, BuyerInMemoryRepository>()
							.AddSingleton<IInventoryInMemoryRepository, InventoryInMemoryRepository>()
							.AddSingleton<IOrderInMemoryRepository, OrderInMemoryRepository>()
							.AddSingleton<ISupplierInMemoryRepository, SupplierInMemoryRepository>()
				)
				.ConfigureLogging(logging =>
					{
						logging.ClearProviders();
						logging.AddNLog("nlog.config");
					});
							
		static void AddSuppliers(ISupplierService supplierService)
        {
			Console.WriteLine("Adding suppliers ... ");

			// Add three suppliers
			for (int i = 1; i <= 3; i++)
			{
				supplierService.AddSupplier(new Supplier()
				{
					Id = i,
					Name = $"Supplier{i}"
				});
			}

			Console.WriteLine("Suppliers added");
		}

		static void AddArticles(IArticleService articleService)
        {
			Console.WriteLine("Adding articles ... ");

			Random random = new Random();

			// Add three articles
			for (int i = 1; i <= 3; i++)
			{
				var ean = "";
				for (int j = 0; j < 13; j++)
				{
					ean += $"{random.Next(0,9)}";
				}

				articleService.AddArticle(new Article()
				{
					EAN = ean,
					Name = $"Article{i}"
				});

			}

			Console.WriteLine("Articles added");

		}

		static void AddInventory(ISupplierService supplierService, IArticleService articleService)
        {
			Console.WriteLine("Adding inventory ...");

			IList<Supplier> suppliers = supplierService.GetAllSuppliers();
			IList<Article> articles = articleService.GetAllArticles();


			for (int i = 0; i < suppliers.Count; i++)
			{
				for (int j = 0; j < articles.Count; j++)
				{
					var quantity = i + j + 2;
					var price = 2 * quantity;

					articles[j].Price = price;
					articles[j].Quantity = quantity;

					supplierService.AddArticleToSupplierInventory(suppliers[i], articles[j]);
				}

			}

			Console.WriteLine("Inventory added");
		}

		static void AddBuyers(IBuyerService buyerService)
        {
			Console.WriteLine("Adding buyers ... ");

			for (int i = 1; i <= 3; i++)
			{
				buyerService.AddBuyer(new Buyer()
				{
					Name = $"Buyer{i}"
				});
			}

			Console.WriteLine("Buyers added");
		}

		static void OrderAndSellArticle(ISupplierService supplierService, IBuyerService buyerService, IShopService shopService, IArticleService articleService)
        {
			int id = 1;
			string ean;
			// Get article for order and sell article example (EAN is needed, that's why this call is made)
			Article article = articleService.GetArticle(id);

			double maxExpectedPrice = 8.00;
			ean = article.EAN;

			Console.WriteLine($"Ordering article with EAN={article.EAN} ...");
			article = supplierService.OrderArticle(article.EAN, maxExpectedPrice);
			
			if (article == null)
            {
				Console.WriteLine($"Article with EAN={ean} for price less or equal then {maxExpectedPrice} not found");
			}
			else
            {
				Console.WriteLine($"Article with EAN={article.EAN} for price less or equal then {maxExpectedPrice} found");
				Buyer buyer = buyerService.GetBuyer(1);

				try
				{
					Console.WriteLine($"Selling article with id={article.Id} to buyer {buyer.Name} ...");
					shopService.SellArticle(article, buyer);
					Console.WriteLine($"Sold article with i={article.Id} to buyer {buyer.Name}");
				}
				catch
				{
					Console.WriteLine($"Cannot sell article to buyer, returning it to supplier ... ");
					supplierService.ReturnArticleToSupplier(article);
					Console.WriteLine($"Article with id={article.Id} returned to supplier");
				}
			}
			
		}

		static void GetAndPrintArticle(IArticleService articleService, int id)
        {
			Console.WriteLine($"Finding article with id={id} ...");
			Article article = articleService.GetArticle(id);

			if (article != null)
            {
				Console.WriteLine($"Found article with Id={article.Id}");
			}
			else
            {
				Console.WriteLine($"Didn't find article with Id={id}");
			}
			
		}

	}
}
