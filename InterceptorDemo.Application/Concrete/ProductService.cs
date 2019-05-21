using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Core.Models;
using System.Collections.Generic;

namespace InterceptorDemo.Application.Concrete
{
	public class ProductService : IProductService
	{
		public List<Product> GetProducts()
		{
			var a = 5;
			var b = 0;
			var c = a / b;

			return null;
		}
	}
}
