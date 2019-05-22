using Autofac;
using Microsoft.Extensions.Configuration;

namespace InterceptorDemo.WebUI
{
	public class WebAppModule : Module
	{
		private readonly IConfiguration _configuration;
		public WebAppModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		protected override void Load(ContainerBuilder builder) { }
	}
}
