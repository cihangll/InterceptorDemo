using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects;
using InterceptorDemo.Core.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Application.Concrete
{
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


		public List<Product> GetProducts()
		{
			return _products;
		}

		[MeasureDuration(3)]
		public async Task<List<Product>> GetProductsAsync()
		{
			await Task.Delay(4000);
			return _products;
		}

		public void ThrowError()
		{
			throw new System.Exception($"test exception from {MethodBase.GetCurrentMethod().Name}");
		}

		[MeasureDuration(2)]
		public async Task ThrowErrorAsync()
		{
			await Task.Delay(4000);
			throw new System.Exception($"test exception from {MethodBase.GetCurrentMethod().Name}");
		}

		[MeasureDuration(3)]
		public async Task<string> ThrowErrorAsyncWithReturnType()
		{
			await Task.Delay(4000).ContinueWith(x =>
			{
				throw new System.Exception($"test exception from {MethodBase.GetCurrentMethod().Name}");

			});
			return "";
		}
	}
}
