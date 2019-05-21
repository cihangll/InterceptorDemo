using Castle.Core.Logging;
using InterceptorDemo.Application.Abstract;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult Index()
		{
			return Json(_productService.GetProducts());
		}
	}
}