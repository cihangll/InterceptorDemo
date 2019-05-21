using Autofac;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects;

namespace InterceptorDemo.Core
{
	public class CoreModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ExceptionHandlingInterceptor>();
			builder.RegisterType<LogInterceptor>();
		}
	}
}
