using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Application.ValidationRules.FluentValidation;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.CacheAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects;
using InterceptorDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterceptorDemo.Application.Concrete
{
	[Log]
	[ExceptionHandling]
	[MeasureDuration(isActive: false)]
	public class ProductService : IProductService
	{
		private static readonly List<Product> _products = new List<Product>()
		{
			new Product() { CategoryId = 1, ProductId = 1,ProductName = "product 1", QuantityPerUnit = "1", UnitPrice = 5},
			new Product() { CategoryId = 1, ProductId = 2,ProductName = "product 2", QuantityPerUnit = "2", UnitPrice = 50},
			new Product() { CategoryId = 1, ProductId = 3,ProductName = "product 3", QuantityPerUnit = "3", UnitPrice = 50},
			new Product() { CategoryId = 1, ProductId = 4,ProductName = "product 4", QuantityPerUnit = "4", UnitPrice = 500},
			new Product() { CategoryId = 1, ProductId = 5,ProductName = "product 5", QuantityPerUnit = "5", UnitPrice = 5000}
		};

		[Cache(300)]
		[MeasureDuration(3)]
		public List<Product> GetProducts()
		{
			//For test
			Task.Delay(3000).Wait();
			return _products;
		}

		[Cache(300)]
		[MeasureDuration(3)]
		public async Task<List<Product>> GetProductsAsync()
		{
			//For test
			await Task.Delay(3000);
			return _products;
		}

		[FluentValidation(typeof(ProductValidator))]
		public async Task<string> SaveProduct([NotNull]Product product)
		{
			await Task.Delay(3000);
			return "Product saved.";
		}
	}
}
