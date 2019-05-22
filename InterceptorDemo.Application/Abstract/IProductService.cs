using InterceptorDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterceptorDemo.Application.Abstract
{
	public interface IProductService
	{
		List<Product> GetProducts();
		Task<List<Product>> GetProductsAsync();
		void ThrowError();
		Task ThrowErrorAsync();
		Task<string> ThrowErrorAsyncWithReturnType();
		void NullCheck1(List<Product> arg1);
		Task NullCheck2(string arg1, Product arg2);
		Task<string> NullCheck3(string arg1, Product arg2);
		Task<string> SaveProduct(Product product);
	}
}
