﻿using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects;
using InterceptorDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterceptorDemo.Application.Concrete
{
	[MeasureDuration(10)]
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

		[MeasureDuration(5)]
		public List<Product> GetProducts()
		{
			return _products;
		}

		[MeasureDuration(7)]
		public async Task<List<Product>> GetProductsAsync()
		{
			await Task.Delay(5000);
			return _products;
		}

	}
}
