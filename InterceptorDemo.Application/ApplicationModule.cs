using Autofac;
using Autofac.Extras.DynamicProxy;
using InterceptorDemo.Application.Abstract;
using InterceptorDemo.Application.Concrete;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.CacheAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects;
using InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects;
using System;

namespace InterceptorDemo.Application
{
	public class ApplicationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ProductService>()
				.As<IProductService>()
				.EnableInterfaceInterceptors()
				.InterceptedBy(RegisterAllInterceptors());
		}

		/// <summary>
		/// Order is IMPORTANT.
		/// <para>Example Usage;
		/// 1) Null Check
		/// 2) Validation Check
		/// 3) Start try catch (Exception)
		/// 4) Start measure
		/// 5) Logging
		/// 6) Caching
		/// </para>
		/// </summary>
		/// <returns></returns>
		private Type[] RegisterAllInterceptors()
		{
			return new Type[]{
				typeof(NullCheckValidationInterceptor),
				typeof(FluentValidationInterceptor),
				typeof(ExceptionHandlingInterceptor),
				typeof(MeasureDurationInterceptor),
				typeof(LogInterceptor),
				typeof(CacheInterceptor)
			};
		}
	}
}
