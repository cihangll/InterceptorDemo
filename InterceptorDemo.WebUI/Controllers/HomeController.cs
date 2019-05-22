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

		[HttpGet("test3")]
		public IActionResult Test3()
		{
			_productService.ThrowError();
			return Json("");
		}

		[HttpGet("test4")]
		public async Task<IActionResult> Test4()
		{
			await _productService.ThrowErrorAsync();
			return Json("");
		}

		[HttpGet("test5")]
		public async Task<IActionResult> Test5()
		{
			return Json(await _productService.ThrowErrorAsyncWithReturnType());
		}

		[HttpGet("test6")]
		public IActionResult Test6()
		{
			_productService.NullCheck1(null);
			return Json("");
		}

		[HttpGet("test7")]
		public async Task<IActionResult> Test7()
		{
			await _productService.NullCheck2(null, null);
			return Json("");
		}

		[HttpGet("test8")]
		public async Task<IActionResult> Test8()
		{
			return Json(await _productService.NullCheck3("this will be run.", null));
		}
	}
}