using InterceptorDemo.Core.Models;
using System.Collections.Generic;

namespace InterceptorDemo.Application.Abstract
{
	public interface IProductService
	{
		List<Product> GetProducts();
	}
}
