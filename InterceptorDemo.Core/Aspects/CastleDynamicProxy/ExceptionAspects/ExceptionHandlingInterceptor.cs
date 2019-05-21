using Castle.Core.Logging;
using Castle.DynamicProxy;
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
				_logManager.Error("Argument cannot be null.", e);
				throw;
			}
			catch (Exception e)
			{
				_logManager.Error("Error occured.", e);
				throw;
			}
		}

	}
}
