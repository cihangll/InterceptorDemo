using Castle.Core.Logging;
using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InterceptorDemo.WebUI.Controllers
{
	public class HomeController : Controller
	{

		private readonly IProductService _productService;
		private readonly ILogger _logger;

		public HomeController(IProductService productService, ILogger logger)
		{
			_productService = productService;
			_logger = logger;
		}

		[HttpGet("products")]
		public IActionResult GetProducts()
		{
			return Json(_productService.GetProducts());
		}

		[HttpGet("productsAsync")]
		public async Task<IActionResult> GetProductsAsync()
		{
			return Json(await _productService.GetProductsAsync());
		}

		[HttpGet("saveProduct")]
		public async Task<IActionResult> PostProduct()
		{
			//For test.
			return Json(await _productService.SaveProduct(new Product()
			{
				//CategoryId = 5,
				ProductId = 2,
				ProductName = "test",
				//QuantityPerUnit = "4",
				UnitPrice = 400
			}));
		}
	}
}