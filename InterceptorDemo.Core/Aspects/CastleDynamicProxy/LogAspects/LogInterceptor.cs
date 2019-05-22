using Castle.Core.Logging;
using Castle.DynamicProxy;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging;
using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects
{
	[Serializable]
	public class LogInterceptor : IInterceptor
	{

		private LogAttribute attribute;
		private readonly ILogger _logManager;

		public LogInterceptor(ILogger logManager)
		{
			_logManager = logManager;
		}

		public void Intercept(IInvocation invocation)
		{
			if (!InterceptorHelper.DecideToIntercept(invocation, out attribute))
			{
				invocation.Proceed();
				return;
			}

			_logManager.Info(LogHelper.CreateLogString(invocation));
			invocation.Proceed();
		}
	}
}
