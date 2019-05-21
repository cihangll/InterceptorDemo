using Castle.Core.Logging;
using Castle.DynamicProxy;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging;
using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects
{
	[Serializable]
	public class ExceptionHandlingInterceptor : IInterceptor
	{
		private readonly ILogger _logManager;

		public ExceptionHandlingInterceptor(ILogger logManager)
		{
			_logManager = logManager;
		}

		public void Intercept(IInvocation invocation)
		{
			try
			{
				invocation.Proceed();
			}
			catch (ArgumentNullException e)
			{
				_logManager.Error($"Argument cannot be null.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
			catch (Exception e)
			{
				_logManager.Error($"Error occured.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
		}

	}
}
