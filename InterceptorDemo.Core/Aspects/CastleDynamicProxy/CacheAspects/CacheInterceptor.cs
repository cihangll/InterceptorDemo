using Castle.Core.Logging;
using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.CacheAspects
{
	[Serializable]
	public class CacheInterceptor : IInterceptor
	{
		private static readonly MethodInfo handleAsyncMethodInfo = typeof(CacheInterceptor).GetMethod("InterceptAsynchronousWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private CacheAttribute attribute;

		private readonly ILogger _logger;
		private readonly object _lockObject = new object();

		public CacheInterceptor(ILogger logger)
		{
			_logger = logger;
		}

		public void Intercept(IInvocation invocation)
		{

			if (!InterceptorHelper.DecideToIntercept(invocation, out attribute))
			{
				invocation.Proceed();
				return;
			}

			var delegateType = InterceptorHelper.GetDelegateType(invocation);
			if (delegateType == MethodType.Synchronous)
			{
				InterceptSynchronous(invocation);
			}
			if (delegateType == MethodType.AsyncAction)
			{
				InterceptAsynchronous(invocation);
			}
			if (delegateType == MethodType.AsyncFunction)
			{
				InterceptAsynchronousWithResultUsingReflection(invocation);
			}
		}

		private void InterceptSynchronous(IInvocation invocation)
		{
			invocation.Proceed();
		}

		private void InterceptAsynchronous(IInvocation invocation)
		{
			invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
		}

		private void InterceptAsynchronousWithResult<TResult>(IInvocation invocation)
		{
			invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
		}

		private void InterceptAsynchronousWithResultUsingReflection(IInvocation invocation)
		{
			var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
			var methodInfo = handleAsyncMethodInfo.MakeGenericMethod(resultType);
			methodInfo.Invoke(this, new[] { invocation });
		}

		private async Task InternalInterceptAsynchronous(IInvocation invocation)
		{
			invocation.Proceed();
			var task = (Task)invocation.ReturnValue;
			await task;
		}

		private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
		{
			invocation.Proceed();
			var task = (Task<TResult>)invocation.ReturnValue;
			TResult result = await task;

			return result;
		}
	}
}
