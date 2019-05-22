using InterceptorDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterceptorDemo.Application.Abstract
{
	public interface IProductService
	{
		List<Product> GetProducts();
		Task<List<Product>> GetProductsAsync();
	}
}
