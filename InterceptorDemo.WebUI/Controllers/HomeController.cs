using Castle.Core.Logging;
using InterceptorDemo.Application.Abstract;
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

		[HttpGet("test1")]
		public IActionResult Test1()
		{
			return Json(_productService.GetProducts());
		}

		[HttpGet("test2")]
		public async Task<IActionResult> Test2()
		{
			return Json(await _productService.GetProductsAsync());
		}

	}
}