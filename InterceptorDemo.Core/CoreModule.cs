using Autofac;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.CastleCoreSerilog;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.Config;

namespace InterceptorDemo.Core
{
	public class CoreModule : Module
	{
		private readonly CastleCoreSerilogConfig _config;
		public CoreModule(CastleCoreSerilogConfig config)
		{
			_config = config;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ExceptionHandlingInterceptor>();
			builder.RegisterType<LogInterceptor>();
			builder.RegisterType<MeasureDurationInterceptor>();
			builder.RegisterType<NullCheckValidationInterceptor>();

			builder.Register(x => SerilogInstance.CreateCastleCoreLogger(_config)).SingleInstance();
		}
	}
}
