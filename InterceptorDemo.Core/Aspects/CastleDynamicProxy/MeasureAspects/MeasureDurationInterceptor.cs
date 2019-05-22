using Castle.Core.Logging;
using Castle.DynamicProxy;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects
{
	[Serializable]
	public class MeasureDurationInterceptor : IInterceptor
	{
		private static readonly MethodInfo handleAsyncMethodInfo = typeof(MeasureDurationInterceptor).GetMethod("InterceptAsynchronousWithResult", BindingFlags.Instance | BindingFlags.NonPublic);

		private readonly ILogger _logger;

		public MeasureDurationInterceptor(ILogger logger)
		{
			_logger = logger;
		}

		public void Intercept(IInvocation invocation)
		{

			if (!InterceptorHelper.DecideToIntercept<MeasureDurationAttribute>(invocation))
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
			var stopwatch = Stopwatch.StartNew();

			invocation.Proceed();

			stopwatch.Stop();

			LogExecutionTime(invocation, stopwatch, 5);
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
			var stopwatch = Stopwatch.StartNew();

			invocation.Proceed();
			var task = (Task)invocation.ReturnValue;
			await task;

			stopwatch.Stop();

			LogExecutionTime(invocation, stopwatch, 5);
		}

		private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
		{
			var stopwatch = Stopwatch.StartNew();

			invocation.Proceed();
			var task = (Task<TResult>)invocation.ReturnValue;
			TResult result = await task;

			stopwatch.Stop();

			LogExecutionTime(invocation, stopwatch, 5);

			return result;
		}

		private void LogExecutionTime(IInvocation invocation, Stopwatch stopwatch, int logafterseconds)
		{
			if (stopwatch.Elapsed.TotalSeconds >= logafterseconds)
			{
				_logger.Warn($"Measure Duration Interceptor: {invocation.MethodInvocationTarget.Name} executed in {stopwatch.Elapsed.TotalMilliseconds.ToString("0.000")} milliseconds.({stopwatch.Elapsed.TotalSeconds.ToString("0.000")} seconds)");
			}
		}
	}
}
