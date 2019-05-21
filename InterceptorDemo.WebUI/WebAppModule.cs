using Autofac;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.CastleCoreSerilog;
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
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(x => SerilogInstance.CreateCastleCoreLogger(_configuration)).SingleInstance();
		}
	}
}
